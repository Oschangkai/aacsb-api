namespace AACSB.WebApi.Application.Identity.Roles;

public class RoleDetailDto : RoleDto
{
    public List<string>? Claims { get; set; }
}