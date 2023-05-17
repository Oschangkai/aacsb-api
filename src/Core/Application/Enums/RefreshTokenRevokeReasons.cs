namespace AACSB.WebApi.Application.Enums;

public static class RefreshTokenRevokeReasons
{
    public const string ReplacedByAnother = "Replaced by New Token";
    public const string LoggedOut = "User Logged out";
    public const string PermissionChanged = "Permission Changed";
    public const string GroupChanged = "Group Changed";
}