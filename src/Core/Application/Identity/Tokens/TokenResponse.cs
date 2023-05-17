namespace AACSB.WebApi.Application.Identity.Tokens;

public record TokenResponse(string Token, string RefreshToken, long ExpireOn);