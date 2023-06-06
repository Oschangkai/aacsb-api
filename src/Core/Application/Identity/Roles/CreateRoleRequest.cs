namespace AACSB.WebApi.Application.Identity.Roles;

public class CreateRoleRequest
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public string[]? Claims { get; set; }
}

public class CreateRoleRequestValidator : CustomValidator<CreateRoleRequest>
{
    public CreateRoleRequestValidator(IRoleService roleService, IStringLocalizer<CreateRoleRequestValidator> T) =>
        RuleFor(r => r.Name)
            .NotEmpty()
            .MustAsync(async (role, name, _) => !await roleService.ExistsAsync(name, string.Empty))
                .WithMessage(T["Similar Role already exists."]);
}