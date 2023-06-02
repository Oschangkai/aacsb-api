namespace AACSB.WebApi.Application.Identity.Tokens;

public record RefreshTokenRequest(string Token, string RefreshToken);
public record RefreshTokenBodyRequest(string RefreshToken);