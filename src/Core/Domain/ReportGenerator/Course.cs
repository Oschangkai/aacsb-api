using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace AACSB.WebApi.Domain.ReportGenerator;

[Table("Courses", Schema="ReportGenerator")]
public class Course : AuditableEntity, IAggregateRoot
{
    public short Semester { get; set; }

    [MaxLength(50)]
    public string Code { get; set; }

    [MaxLength(200)]
    public string Name { get; set; }

    [MaxLength(500)]
    public string EnglishName { get; set; }

    [Precision(6, 4)]
    public decimal Credit { get; set; }

    [Comment("必修/選修")]
    public bool? Required { get; set; }

    [Comment("全半學年")]
    public bool? Year { get; set; }

    [Comment("節次，M1, T6, W3，以逗點分隔")]
    [MaxLength(256)]
    public string? Time { get; set; }

    [Comment("教室")]
    [MaxLength(50)]
    public string? ClassRoomNo { get; set; }

    [Comment("課程說明")]
    [MaxLength(500)]
    public string? Contents { get; set; }

    public Guid? DepartmentId { get; set; }
    public Department? Department { get; set; }
    public Guid? ImportSignatureId { get; set; }
    public ImportSignature? ImportSignature { get; set; }
    public Guid? DisciplineId { get; set; }
    public Discipline? Discipline { get; set; }

    public ICollection<CourseTeacher>? Teachers { get; set; }

    public Course()
    {
        Semester = 0;
        Code = string.Empty;
        Name = string.Empty;
        EnglishName = string.Empty;
        Credit = decimal.Zero;
        Required = false;
        Year = false;
        Time = string.Empty;
    }
}