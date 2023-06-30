using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AACSB.WebApi.Domain.ReportGenerator;

[Table("Disciplines", Schema="ReportGenerator")]
public class Discipline : AuditableEntity, IAggregateRoot
{
    public byte Code { get; private set; }

    [MaxLength(128)]
    public string Name { get; private set; }

    public ICollection<Course>? Courses { get; private set; }

    public Discipline(byte code, string name)
    {
        Code = code;
        Name = name;
    }
}