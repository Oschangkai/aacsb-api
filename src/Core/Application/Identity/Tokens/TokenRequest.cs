using AACSB.WebApi.Application.Enums;

namespace AACSB.WebApi.Application.Identity.Tokens;

public record TokenRequest(string Type, string? Email, string? Password, string? Token);

public class TokenRequestValidator : CustomValidator<TokenRequest>
{
    public TokenRequestValidator(IStringLocalizer<TokenRequestValidator> T)
    {
        RuleFor(p => p.Type).Cascade(CascadeMode.Stop)
            .NotEmpty()
            .Must(BeTokenExchangeTypes)
                .WithMessage(T["Invalid Exchange Type."]);

        RuleFor(p => p.Email).Cascade(CascadeMode.Stop)
            .NotEmpty()
            .EmailAddress()
            .When(p => p.Type == TokenExchangeTypes.UserNamePassword)
                .WithMessage(T["Invalid Email Address."]);

        RuleFor(p => p.Password).Cascade(CascadeMode.Stop)
            .NotEmpty()
            .When(p => p.Type == TokenExchangeTypes.UserNamePassword);

        RuleFor(p => p.Token).Cascade(CascadeMode.Stop)
            .NotEmpty()
            .When(p => p.Type == TokenExchangeTypes.RefreshToken);
    }

    private static bool BeTokenExchangeTypes(string type)
        => type.ToLower() is TokenExchangeTypes.RefreshToken
            or TokenExchangeTypes.UserNamePassword
            or TokenExchangeTypes.AuthorizationCode;
}