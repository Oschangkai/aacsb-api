using System.ComponentModel.DataAnnotations.Schema;

namespace AACSB.WebApi.Infrastructure.Identity;

[Table("RefreshToken", Schema = "Identity")]
public class RefreshToken
{
    public int Id { get; set; }
    public string? Token { get; set; }
    public DateTime Expires { get; set; }
    public bool IsExpired => DateTime.UtcNow >= Expires;
    public DateTime CreatedOn { get; set; }
    public DateTime? RevokedOn { get; set; }
    public string? RevokeReason { get; set; }
    public string? ReplacedByToken { get; set; }
    public bool IsActive => RevokedOn == null && !IsExpired && DateTime.UtcNow >= CreatedOn;
}