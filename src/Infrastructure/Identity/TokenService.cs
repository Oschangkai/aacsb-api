using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AACSB.WebApi.Application.Common.Exceptions;
using AACSB.WebApi.Application.Enums;
using AACSB.WebApi.Application.Identity.Tokens;
using AACSB.WebApi.Infrastructure.Auth;
using AACSB.WebApi.Infrastructure.Auth.Jwt;
using AACSB.WebApi.Infrastructure.Multitenancy;
using AACSB.WebApi.Shared.Authorization;
using AACSB.WebApi.Shared.Multitenancy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AACSB.WebApi.Infrastructure.Identity;

internal class TokenService : ITokenService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IStringLocalizer _t;
    private readonly SecuritySettings _securitySettings;
    private readonly JwtSettings _jwtSettings;
    private readonly AACSBTenantInfo? _currentTenant;
    private readonly ILogger<TokenService> _logger;

    public TokenService(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        IOptions<JwtSettings> jwtSettings,
        IStringLocalizer<TokenService> localizer,
        ILogger<TokenService> logger,
        AACSBTenantInfo? currentTenant,
        IOptions<SecuritySettings> securitySettings)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _t = localizer;
        _jwtSettings = jwtSettings.Value;
        _currentTenant = currentTenant;
        _securitySettings = securitySettings.Value;
        _logger = logger;
    }

    public async Task<TokenResponse> GetTokenAsync(TokenRequest request, string ipAddress, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(_currentTenant?.Id)
            || await _userManager.FindByEmailAsync(request.Email.Trim().Normalize()) is not { } user
            || !await _userManager.CheckPasswordAsync(user, request.Password))
        {

            throw new UnauthorizedException(_t["Authentication Failed."]);
        }

        if (!user.IsActive)
        {
            throw new UnauthorizedException(_t["User Not Active. Please contact the administrator."]);
        }

        if (_securitySettings.RequireConfirmedAccount && !user.EmailConfirmed)
        {
            throw new UnauthorizedException(_t["E-Mail not confirmed."]);
        }

        if (_currentTenant.Id != MultitenancyConstants.Root.Id)
        {
            if (!_currentTenant.IsActive)
            {
                throw new UnauthorizedException(_t["Tenant is not Active. Please contact the Application Administrator."]);
            }

            if (DateTime.UtcNow > _currentTenant.ValidUpto)
            {
                throw new UnauthorizedException(_t["Tenant Validity Has Expired. Please contact the Application Administrator."]);
            }
        }

        return await GenerateTokensAndUpdateUser(user);
    }

    public async Task<TokenResponse> RefreshTokenAsync(RefreshTokenRequest request)
    {
        var userPrincipal = GetPrincipalFromExpiredToken(request.Token);
        string? userId = userPrincipal.GetUserId();
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            throw new ApiAuthenticateException(_t["Authentication Failed."]);
        }

        if (user.RefreshTokens != null)
        {
            var refreshToken = user.RefreshTokens.SingleOrDefault(t => t.Token == request.RefreshToken);
            if (refreshToken is null or { IsExpired: true })
            {
                throw new ApiAuthenticateException($"Login Timeout, Please Login Again");
            }
        }

        var response = await GenerateTokensAndUpdateUser(user);
        await RevokeRefreshToken(RefreshTokenRevokeReasons.ReplacedByAnother, request.RefreshToken, response.RefreshToken);
        return response;
    }

    private async Task<TokenResponse> GenerateTokensAndUpdateUser(ApplicationUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);
        var permissionClaims = new List<Claim>();
        foreach (string role in roles)
        {
            var r = await _roleManager.FindByNameAsync(role);
            var rc = await _roleManager.GetClaimsAsync(r);
            permissionClaims.AddRange(rc);
        }

        string token = GenerateJwt(user, permissionClaims);

        var refreshToken = GenerateRefreshToken();
        user.RefreshTokens ??= new List<RefreshToken>();
        user.RefreshTokens.Add(refreshToken);

        await _userManager.UpdateAsync(user);

        return new TokenResponse(token, refreshToken.Token, ((DateTimeOffset)refreshToken.Expires).ToUnixTimeSeconds());
    }

    private string GenerateJwt(ApplicationUser user, List<Claim> permissionClaims) =>
        GenerateEncryptedToken(GetSigningCredentials(), GetClaims(user, permissionClaims));

    private IEnumerable<Claim> GetClaims(ApplicationUser user, List<Claim> permissionClaims) =>
        new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(AACSBClaims.Fullname, $"{user.FirstName} {user.LastName}"),
            new(AACSBClaims.Tenant, _currentTenant!.Id),
            new(AACSBClaims.ImageUrl, user.ImageUrl ?? string.Empty),
        }.Union(permissionClaims.GroupBy(c => c.Value).Select(c => c.First()));

    private RefreshToken GenerateRefreshToken()
    {
        return new RefreshToken()
        {
            Token = RandomTokenString(),
            Expires = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationInDays),
            CreatedOn = DateTime.UtcNow
        };
    }

    private string RandomTokenString()
    {
        using var rngCryptoServiceProvider = RandomNumberGenerator.Create();
        byte[] randomBytes = new byte[40];
        rngCryptoServiceProvider.GetBytes(randomBytes);

        // convert random bytes to hex string
        return BitConverter.ToString(randomBytes).Replace("-", string.Empty);
    }

    private string GenerateEncryptedToken(SigningCredentials signingCredentials, IEnumerable<Claim> claims)
    {
        var token = new JwtSecurityToken(
           audience: _jwtSettings.Audience,
           issuer: _jwtSettings.Issuer,
           claims: claims,
           expires: DateTime.UtcNow.AddMinutes(_jwtSettings.TokenExpirationInMinutes),
           notBefore: DateTime.UtcNow,
           signingCredentials: signingCredentials);
        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }

    private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key)),
            ValidateIssuer = true,
            ValidIssuer = _jwtSettings.Issuer,
            ValidateAudience = true,
            ValidAudience = _jwtSettings.Audience,
            RoleClaimType = ClaimTypes.Role,
            ClockSkew = TimeSpan.Zero,
            ValidateLifetime = false
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(
                SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
        {
            throw new ApiAuthenticateException(_t["Invalid Token."]);
        }

        return principal;
    }

    private SigningCredentials GetSigningCredentials()
    {
        byte[] secret = Encoding.UTF8.GetBytes(_jwtSettings.Key);
        return new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256);
    }

    public async Task RevokeRefreshToken(string reason, string refreshTokenString, string? replacedToken = null)
    {
        if (string.IsNullOrEmpty(refreshTokenString)) return;

        var user = _userManager.Users
            .Include(u => u.RefreshTokens)
            .FirstOrDefault(u
                => u.RefreshTokens != null && u.RefreshTokens.Any(rt => rt.Token == refreshTokenString));

        var refreshToken = user?.RefreshTokens?.Find(rt => rt.Token == refreshTokenString);
        if (refreshToken is null) return;

        refreshToken.RevokedOn = DateTime.UtcNow;
        refreshToken.RevokeReason = reason;
        if (!string.IsNullOrEmpty(replacedToken))
        {
            refreshToken.ReplacedByToken = replacedToken;
        }

        await _userManager.UpdateAsync(user);
    }
}