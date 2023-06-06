using AACSB.WebApi.Application.Identity.Roles;

namespace AACSB.WebApi.Host.Controllers.Identity;

public class RolesController : VersionNeutralApiController
{
    private readonly IRoleService _roleService;

    public RolesController(IRoleService roleService) => _roleService = roleService;

    [HttpGet]
    [MustHavePermission(AACSBAction.View, AACSBResource.Roles)]
    [OpenApiOperation("Get a list of all roles.", "")]
    public Task<List<RoleDto>> GetListAsync(CancellationToken cancellationToken)
    {
        return _roleService.GetListAsync(cancellationToken);
    }

    [HttpGet("{id}")]
    [MustHavePermission(AACSBAction.View, AACSBResource.Roles)]
    [OpenApiOperation("Get role details.", "")]
    public Task<RoleDetailDto> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        return _roleService.GetByIdWithPermissionsAsync(id, cancellationToken);
    }

    [HttpGet("{id}/permissions")]
    [MustHavePermission(AACSBAction.View, AACSBResource.RoleClaims)]
    [OpenApiOperation("Get role details with its permissions.", "")]
    public Task<RoleDetailDto> GetByIdWithPermissionsAsync(string id, CancellationToken cancellationToken)
    {
        return _roleService.GetByIdWithPermissionsAsync(id, cancellationToken);
    }

    [HttpPut("{id}/permissions")]
    [MustHavePermission(AACSBAction.Update, AACSBResource.RoleClaims)]
    [OpenApiOperation("Update a role's permissions.", "")]
    public async Task<ActionResult<string>> UpdatePermissionsAsync(string id, UpdateRolePermissionsRequest request, CancellationToken cancellationToken)
    {
        if (id != request.RoleId)
        {
            return BadRequest();
        }

        return Ok(await _roleService.UpdatePermissionsAsync(request, cancellationToken));
    }

    [HttpPost]
    [MustHavePermission(AACSBAction.Create, AACSBResource.Roles)]
    [OpenApiOperation("Create or update a role.", "")]
    public Task<MessageResponse> RegisterRoleAsync(CreateRoleRequest request)
    {
        return _roleService.CreateAsync(request);
    }

    [HttpPatch]
    [MustHavePermission(AACSBAction.Update, AACSBResource.Roles)]
    [OpenApiOperation("Update a role.", "")]
    public Task<MessageResponse> UpdateRoleAsync(UpdateRoleRequest request)
    {
        return _roleService.UpdateAsync(request);
    }

    [HttpDelete("{id}")]
    [MustHavePermission(AACSBAction.Delete, AACSBResource.Roles)]
    [OpenApiOperation("Delete a role.", "")]
    public Task<string> DeleteAsync(string id)
    {
        return _roleService.DeleteAsync(id);
    }
}