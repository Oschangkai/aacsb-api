using System.Net.Http.Headers;
using AACSB.WebApi.Application.Enums;
using AACSB.WebApi.Application.Identity.Tokens;

namespace AACSB.WebApi.Host.Controllers.Identity;

public sealed class TokensController : VersionNeutralApiController
{
    private readonly ITokenService _tokenService;

    public TokensController(ITokenService tokenService) => _tokenService = tokenService;

    [HttpPost("login")]
    [AllowAnonymous]
    [TenantIdHeader]
    [OpenApiOperation("Request an access token using credentials.", "")]
    public Task<TokenResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        return _tokenService.GetTokenAsync(request, GetIpAddress(), cancellationToken);
    }

    [HttpPost]
    [AllowAnonymous]
    [TenantIdHeader]
    [OpenApiOperation("Request an access token.", "")]
    public Task<TokenResponse> GetTokenAsync(TokenRequest request, [FromHeader] string? authorization, CancellationToken cancellationToken)
    {
        if (request.Type == TokenExchangeTypes.UserNamePassword)
            return _tokenService.GetTokenAsync(new LoginRequest(request.Email!, request.Password!), GetIpAddress(), cancellationToken);

        if (request.Type != TokenExchangeTypes.RefreshToken)
            throw new InvalidOperationException("Invalid token.");
        if (!AuthenticationHeaderValue.TryParse(authorization, out var headerValue))
            throw new InvalidOperationException("Invalid token.");

        string? scheme = headerValue.Scheme; // Bearer
        string? parameter = headerValue.Parameter; // token
        return _tokenService.RefreshTokenAsync(new RefreshTokenRequest(parameter!, request.Token));
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
    [TenantIdHeader]
    [OpenApiOperation("Request an access token using a refresh token.", "")]
    [ApiConventionMethod(typeof(AACSBApiConventions), nameof(AACSBApiConventions.Search))]
    public Task<TokenResponse> RefreshAsync(RefreshTokenBodyRequest token, [FromHeader] string authorization)
    {
        if (!AuthenticationHeaderValue.TryParse(authorization, out var headerValue))
            throw new InvalidOperationException("Invalid token request.");

        string? scheme = headerValue.Scheme; // Bearer
        string? parameter = headerValue.Parameter; // token
        return _tokenService.RefreshTokenAsync(new RefreshTokenRequest(parameter!, token.RefreshToken));
    }

    [AllowAnonymous]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout(LogoutRequest request)
    {
        await _tokenService.RevokeRefreshToken(RefreshTokenRevokeReasons.LoggedOut, request.token);
        return Accepted();
    }

    private string GetIpAddress() =>
        Request.Headers.ContainsKey("X-Forwarded-For")
            ? Request.Headers["X-Forwarded-For"]
            : HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString() ?? "N/A";
}