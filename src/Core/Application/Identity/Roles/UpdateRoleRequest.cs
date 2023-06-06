namespace AACSB.WebApi.Application.Identity.Roles;

public class UpdateRoleRequest
{
    public string? Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public string[]? Claims { get; set; }
}

public class UpdateRoleRequestValidator : CustomValidator<UpdateRoleRequest>
{
    public UpdateRoleRequestValidator(IRoleService roleService, IStringLocalizer<UpdateRoleRequestValidator> T)
    {
        RuleFor(r => r.Id)
            .NotEmpty();
        RuleFor(r => r.Name)
            .NotEmpty();
    }
}