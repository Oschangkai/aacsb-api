using AACSB.WebApi.Shared.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace AACSB.WebApi.Infrastructure.Auth.Permissions;

public class MustHavePermissionAttribute : AuthorizeAttribute
{
    public MustHavePermissionAttribute(string action, string resource) =>
        Policy = AACSBPermission.NameFor(action, resource);
}