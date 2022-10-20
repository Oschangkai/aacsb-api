using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AACSB.WebApi.Domain.ReportGenerator;

[Table("ImportSignatures", Schema="ReportGenerator")]
public class ImportSignature : AuditableEntity, IAggregateRoot
{
    [MaxLength(200)]
    public string Name { get; private set; }
    [MaxLength(500)]
    public string? Description { get; private set; }

    public ICollection<Course>? Courses { get; }
    public ICollection<Teacher>? Teachers { get; }

    public ImportSignature(string name, string description)
    {
        Name = name;
        Description = description;
    }
}