using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace AACSB.WebApi.Domain.ReportGenerator;

[Table("Courses", Schema="ReportGenerator")]
public class Course : AuditableEntity, IAggregateRoot
{
    [Precision(5, 0)]
    public decimal Semester { get; private set; }
    [MaxLength(50)]
    public string Code { get; private set; }
    [MaxLength(200)]
    public string Name { get; private set; }
    [MaxLength(500)]
    public string EnglishName { get; private set; }
    [Precision(6, 4)]
    public decimal Credit { get; private set; }
    [Comment("必修/選修")]
    public bool Required { get; private set; }
    [Comment("全半學年")]
    public bool Year { get; private set; }
    [Comment("節次，M1, T6, W3，以逗點分隔")]
    [MaxLength(500)]
    public string Time { get; private set; }

    public Guid? ImportSignatureId { get; private set; }
    public ImportSignature? ImportSignature { get; private set; }
    public Guid? DisciplineId { get; private set; }
    public Discipline? Discipline { get; private set; }
    public ICollection<Teacher>? Teachers { get; private set; }

    public Course(decimal semester, string code, string name, string englishName, decimal credit, bool required, bool year, string time)
    {
        Semester = semester;
        Code = code;
        Name = name;
        EnglishName = englishName;
        Credit = credit;
        Required = required;
        Year = year;
        Time = time;
    }
}