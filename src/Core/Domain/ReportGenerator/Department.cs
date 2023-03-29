using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AACSB.WebApi.Domain.ReportGenerator;

[Table("Departments", Schema="ReportGenerator")]
public class Department : AuditableEntity, IAggregateRoot
{
    [MaxLength(80)]
    public string? Name { get; set; }

    [MaxLength(80)]
    public string? EnglishName { get; set; }

    [MaxLength(10)]
    public string? Abbreviation { get; set; }
}