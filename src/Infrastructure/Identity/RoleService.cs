using System.Security.Claims;
using Finbuckle.MultiTenant;
using AACSB.WebApi.Application.Common.Events;
using AACSB.WebApi.Application.Common.Exceptions;
using AACSB.WebApi.Application.Common.Interfaces;
using AACSB.WebApi.Application.Common.Models;
using AACSB.WebApi.Application.Identity.Roles;
using AACSB.WebApi.Domain.Identity;
using AACSB.WebApi.Infrastructure.Persistence.Context;
using AACSB.WebApi.Shared.Authorization;
using AACSB.WebApi.Shared.Multitenancy;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace AACSB.WebApi.Infrastructure.Identity;

internal class RoleService : IRoleService
{
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _db;
    private readonly IStringLocalizer _t;
    private readonly ICurrentUser _currentUser;
    private readonly ITenantInfo _currentTenant;
    private readonly IEventPublisher _events;

    public RoleService(
        RoleManager<ApplicationRole> roleManager,
        UserManager<ApplicationUser> userManager,
        ApplicationDbContext db,
        IStringLocalizer<RoleService> localizer,
        ICurrentUser currentUser,
        ITenantInfo currentTenant,
        IEventPublisher events)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _db = db;
        _t = localizer;
        _currentUser = currentUser;
        _currentTenant = currentTenant;
        _events = events;
    }

    public async Task<List<RoleDto>> GetListAsync(CancellationToken cancellationToken) =>
        (await _roleManager.Roles.ToListAsync(cancellationToken))
            .Adapt<List<RoleDto>>();

    public async Task<int> GetCountAsync(CancellationToken cancellationToken) =>
        await _roleManager.Roles.CountAsync(cancellationToken);

    public async Task<bool> ExistsAsync(string roleName, string? excludeId) =>
        await _roleManager.FindByNameAsync(roleName)
            is ApplicationRole existingRole
            && existingRole.Id != excludeId;

    public async Task<RoleDetailDto> GetByIdAsync(string id) =>
        await _db.Roles.SingleOrDefaultAsync(x => x.Id == id) is { } role
            ? role.Adapt<RoleDetailDto>()
            : throw new NotFoundException(_t["Role Not Found"]);

    public async Task<RoleDetailDto> GetByIdWithPermissionsAsync(string roleId, CancellationToken cancellationToken)
    {
        var role = await GetByIdAsync(roleId);

        role.Claims = await _db.RoleClaims
            .Where(c => c.RoleId == roleId && c.ClaimType == AACSBClaims.Permission)
            .Select(c => c.ClaimValue)
            .ToListAsync(cancellationToken);

        return role;
    }

    public async Task<MessageResponse> CreateAsync(CreateRoleRequest request)
    {
        // Create a new role.
        var role = new ApplicationRole(request.Name, request.Description);
        var result = await _roleManager.CreateAsync(role);

        if (!result.Succeeded)
        {
            throw new InternalServerException(_t["Register role failed"], result.GetErrors(_t));
        }

        if (request.Claims != null)
        {
            var r = await _roleManager.FindByNameAsync(role.Name);
            foreach (string claim in request.Claims)
            {
                await _roleManager.AddClaimAsync(r, new Claim(AACSBClaims.Permission, claim));
            }
        }

        await _events.PublishAsync(new ApplicationRoleCreatedEvent(role.Id, role.Name));

        return new MessageResponse(true, string.Format(_t["Role {0} Created."], request.Name));
    }

    public async Task<MessageResponse> UpdateAsync(UpdateRoleRequest request)
    {
        // Update an existing role.
        var role = await _roleManager.FindByIdAsync(request.Id);

        _ = role ?? throw new NotFoundException(_t["Role Not Found"]);

        if (AACSBRoles.IsDefault(role.Name))
        {
            throw new ConflictException(string.Format(_t["Not allowed to modify {0} Role."], role.Name));
        }

        var currentClaims =
            (await _roleManager.GetClaimsAsync(role)).Select(c => c.Value).ToList();
        if (request.Claims?.Any() == true)
        {
            // find revoked claims
            foreach (string c in currentClaims.Except(request.Claims))
            {
                await _roleManager.RemoveClaimAsync(role, new Claim(AACSBClaims.Permission, c));
            }

            // new assigned claims
            foreach (string c in request.Claims.Except(currentClaims))
            {
                await _roleManager.AddClaimAsync(role, new Claim(AACSBClaims.Permission, c));
            }
        }

        role.Name = request.Name;
        role.NormalizedName = request.Name.ToUpperInvariant();
        role.Description = request.Description;
        var result = await _roleManager.UpdateAsync(role);

        if (!result.Succeeded)
        {
            throw new InternalServerException(_t["Update role failed"], result.GetErrors(_t));
        }

        await _events.PublishAsync(new ApplicationRoleUpdatedEvent(role.Id, role.Name));

        return new MessageResponse(true, string.Format(_t["Role {0} Updated."], role.Name));
    }

    public async Task<string> UpdatePermissionsAsync(UpdateRolePermissionsRequest request, CancellationToken cancellationToken)
    {
        var role = await _roleManager.FindByIdAsync(request.RoleId);
        _ = role ?? throw new NotFoundException(_t["Role Not Found"]);
        if (role.Name == AACSBRoles.Admin)
        {
            throw new ConflictException(_t["Not allowed to modify Permissions for this Role."]);
        }

        if (_currentTenant.Id != MultitenancyConstants.Root.Id)
        {
            // Remove Root Permissions if the Role is not created for Root Tenant.
            request.Permissions.RemoveAll(u => u.StartsWith("Permissions.Root."));
        }

        var currentClaims = await _roleManager.GetClaimsAsync(role);

        // Remove permissions that were previously selected
        foreach (var claim in currentClaims.Where(c => !request.Permissions.Any(p => p == c.Value)))
        {
            var removeResult = await _roleManager.RemoveClaimAsync(role, claim);
            if (!removeResult.Succeeded)
            {
                throw new InternalServerException(_t["Update permissions failed."], removeResult.GetErrors(_t));
            }
        }

        // Add all permissions that were not previously selected
        foreach (string permission in request.Permissions.Where(c => !currentClaims.Any(p => p.Value == c)))
        {
            if (!string.IsNullOrEmpty(permission))
            {
                _db.RoleClaims.Add(new ApplicationRoleClaim
                {
                    RoleId = role.Id,
                    ClaimType = AACSBClaims.Permission,
                    ClaimValue = permission,
                    CreatedBy = _currentUser.GetUserId().ToString()
                });
                await _db.SaveChangesAsync(cancellationToken);
            }
        }

        await _events.PublishAsync(new ApplicationRoleUpdatedEvent(role.Id, role.Name, true));

        return _t["Permissions Updated."];
    }

    public async Task<MessageResponse> DeleteAsync(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);

        _ = role ?? throw new NotFoundException(_t["Role Not Found"]);

        if (AACSBRoles.IsDefault(role.Name))
        {
            throw new ConflictException(string.Format(_t["Not allowed to delete {0} Role."], role.Name));
        }

        if ((await _userManager.GetUsersInRoleAsync(role.Name)).Count > 0)
        {
            throw new ConflictException(string.Format(_t["Not allowed to delete {0} Role as it is being used."], role.Name));
        }

        await _roleManager.DeleteAsync(role);

        await _events.PublishAsync(new ApplicationRoleDeletedEvent(role.Id, role.Name));

        return new MessageResponse(true, string.Format(_t["Role {0} Deleted."], role.Name));
    }
}