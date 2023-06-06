namespace AACSB.WebApi.Application.Identity.Roles;

public interface IRoleService : ITransientService
{
    Task<List<RoleDto>> GetListAsync(CancellationToken cancellationToken);

    Task<int> GetCountAsync(CancellationToken cancellationToken);

    Task<bool> ExistsAsync(string roleName, string? excludeId);

    Task<RoleDetailDto> GetByIdAsync(string id);

    Task<RoleDetailDto> GetByIdWithPermissionsAsync(string roleId, CancellationToken cancellationToken);

    Task<MessageResponse> CreateAsync(CreateRoleRequest request);

    Task<MessageResponse> UpdateAsync(UpdateRoleRequest request);

    Task<string> UpdatePermissionsAsync(UpdateRolePermissionsRequest request, CancellationToken cancellationToken);

    Task<string> DeleteAsync(string id);
}