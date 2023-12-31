using AACSB.WebApi.Application.Identity.Users;
using AACSB.WebApi.Application.Identity.Users.Password;

namespace AACSB.WebApi.Host.Controllers.Identity;

public class UsersController : VersionNeutralApiController
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService) => _userService = userService;

    [HttpGet]
    [MustHavePermission(AACSBAction.View, AACSBResource.Users)]
    [OpenApiOperation("Get list of all users.", "")]
    public Task<List<UserListDto>> GetListAsync(CancellationToken cancellationToken)
    {
        return _userService.GetListAsync(cancellationToken);
    }

    [HttpPost("search")]
    [MustHavePermission(AACSBAction.View, AACSBResource.Users)]
    [OpenApiOperation("Get list of all users with filter.", "")]
    public Task<PaginationResponse<UserDetailsDto>> GetListByFilterAsync(UserListFilter filter, CancellationToken cancellationToken)
    {
        return _userService.SearchAsync(filter, cancellationToken);
    }

    [HttpGet("{id}")]
    [MustHavePermission(AACSBAction.View, AACSBResource.Users)]
    [OpenApiOperation("Get a user's details.", "")]
    public Task<UserDetailsDto> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        return _userService.GetAsync(id, cancellationToken);
    }

    [HttpGet("{id}/roles")]
    [MustHavePermission(AACSBAction.View, AACSBResource.UserRoles)]
    [OpenApiOperation("Get a user's roles.", "")]
    public Task<List<UserRoleDto>> GetRolesAsync(string id, CancellationToken cancellationToken)
    {
        return _userService.GetRolesAsync(id, cancellationToken);
    }

    [MustHavePermission(AACSBAction.Update, AACSBResource.Users)]
    [HttpPatch]
    [OpenApiOperation("Edit a user's information.", "")]
    public async Task EditUser(EditUserRequest user, CancellationToken cancellationToken)
    {
        await _userService.EditUserAsync(user, cancellationToken);
    }

    [MustHavePermission(AACSBAction.Delete, AACSBResource.Users)]
    [HttpDelete("{id}")]
    [OpenApiOperation("Delete a user.", "")]
    public async Task<MessageResponse> DeleteUser(string id)
    {
        string userName = await _userService.DeleteUserAsync(id);
        return new MessageResponse(true, $"Deleted User: {userName}");
    }

    [HttpPost("{id}/roles")]
    [ApiConventionMethod(typeof(AACSBApiConventions), nameof(AACSBApiConventions.Register))]
    [MustHavePermission(AACSBAction.Update, AACSBResource.UserRoles)]
    [OpenApiOperation("Update a user's assigned roles.", "")]
    public Task<string> AssignRolesAsync(string id, UserRolesRequest request, CancellationToken cancellationToken)
    {
        return _userService.AssignRolesAsync(id, request, cancellationToken);
    }

    [HttpPost]
    [MustHavePermission(AACSBAction.Create, AACSBResource.Users)]
    [OpenApiOperation("Creates a new user.", "")]
    public Task<string> CreateAsync(CreateUserRequest request)
    {
        // TODO: check if registering anonymous users is actually allowed (should probably be an appsetting)
        // and return UnAuthorized when it isn't
        // Also: add other protection to prevent automatic posting (captcha?)
        return _userService.CreateAsync(request, GetOriginFromRequest());
    }

    [HttpPost("self-register")]
    [TenantIdHeader]
    [AllowAnonymous]
    [OpenApiOperation("Anonymous user creates a user.", "")]
    [ApiConventionMethod(typeof(AACSBApiConventions), nameof(AACSBApiConventions.Register))]
    public Task<string> SelfRegisterAsync(CreateUserRequest request)
    {
        // TODO: check if registering anonymous users is actually allowed (should probably be an appsetting)
        // and return UnAuthorized when it isn't
        // Also: add other protection to prevent automatic posting (captcha?)
        return _userService.CreateAsync(request, GetOriginFromRequest());
    }

    [HttpPost("{id}/toggle-status")]
    [MustHavePermission(AACSBAction.Update, AACSBResource.Users)]
    [ApiConventionMethod(typeof(AACSBApiConventions), nameof(AACSBApiConventions.Register))]
    [OpenApiOperation("Toggle a user's active status.", "")]
    public async Task<ActionResult> ToggleStatusAsync(string id, ToggleUserStatusRequest request, CancellationToken cancellationToken)
    {
        if (id != request.UserId)
        {
            return BadRequest();
        }

        await _userService.ToggleStatusAsync(request, cancellationToken);
        return Ok();
    }

    [HttpGet("confirm-email")]
    [AllowAnonymous]
    [OpenApiOperation("Confirm email address for a user.", "")]
    [ApiConventionMethod(typeof(AACSBApiConventions), nameof(AACSBApiConventions.Search))]
    public Task<string> ConfirmEmailAsync([FromQuery] string tenant, [FromQuery] string userId, [FromQuery] string code, CancellationToken cancellationToken)
    {
        return _userService.ConfirmEmailAsync(userId, code, tenant, cancellationToken);
    }

    [HttpGet("confirm-phone-number")]
    [AllowAnonymous]
    [OpenApiOperation("Confirm phone number for a user.", "")]
    [ApiConventionMethod(typeof(AACSBApiConventions), nameof(AACSBApiConventions.Search))]
    public Task<string> ConfirmPhoneNumberAsync([FromQuery] string userId, [FromQuery] string code)
    {
        return _userService.ConfirmPhoneNumberAsync(userId, code);
    }

    [HttpPost("forgot-password")]
    [AllowAnonymous]
    [TenantIdHeader]
    [OpenApiOperation("Request a password reset email for a user.", "")]
    [ApiConventionMethod(typeof(AACSBApiConventions), nameof(AACSBApiConventions.Register))]
    public Task<string> ForgotPasswordAsync(ForgotPasswordRequest request)
    {
        return _userService.ForgotPasswordAsync(request, GetOriginFromRequest());
    }

    [HttpPost("reset-password")]
    [OpenApiOperation("Reset a user's password.", "")]
    [ApiConventionMethod(typeof(AACSBApiConventions), nameof(AACSBApiConventions.Register))]
    public Task<string> ResetPasswordAsync(ResetPasswordRequest request)
    {
        return _userService.ResetPasswordAsync(request);
    }

    private string GetOriginFromRequest() => $"{Request.Scheme}://{Request.Host.Value}{Request.PathBase.Value}";
}
