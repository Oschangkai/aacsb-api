using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AACSB.WebApi.Domain.ReportGenerator;

[Table("ImportSignatures", Schema="ReportGenerator")]
public class ImportSignature : AuditableEntity, IAggregateRoot
{
    [MaxLength(200)]
    public string Name { get; set; }
    [MaxLength(500)]
    public string? Description { get; set; }

    public ICollection<Course>? Courses { get; }
    public ICollection<Teacher>? Teachers { get; }

    public ImportSignature(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public ImportSignature()
    {
        Name = string.Empty;
        Description = string.Empty;
    }
}