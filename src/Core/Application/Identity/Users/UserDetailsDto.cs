namespace AACSB.WebApi.Application.Identity.Users;

public class UserDetailsDto : UserListDto
{
    public bool EmailConfirmed { get; set; }

    public string? PhoneNumber { get; set; }

    public string? ImageUrl { get; set; }

    public List<string> Roles { get; set; } = new();
}