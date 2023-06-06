namespace AACSB.WebApi.Application.Identity.Users;

public record EditUserRequest(string Id, string FirstName, string LastName, string Email, bool IsActive, string[] Roles);