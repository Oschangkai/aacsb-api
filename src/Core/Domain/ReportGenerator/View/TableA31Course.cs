using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AACSB.WebApi.Domain.ReportGenerator.View;

[Table("TableA31Course", Schema="ReportGenerator")]
public class TableA31Course
{
    [MaxLength(200)]
    public string? Course { get; set; }

    public Guid? CourseId { get; set; }

    public string? CourseCode { get; set; }

    public string? CourseTime { get; set; }

    [MaxLength(200)]
    public string? Teacher { get; set; }

    public Guid? TeacherId { get; set; }

    [MaxLength(200)]
    public string? TeacherEnglishName { get; set; }

    [MaxLength(80)]
    public string? CourseDepartment { get; set; }

    public Guid? CourseDepartmentId { get; set; }

    [MaxLength(10)]
    public string? Degree { get; set; }

    public short? DegreeYear { get; set; }

    [MaxLength(80)]
    public string? Qualification { get; set; }

    public Guid? QualificationId { get; set; }

    [Comment("P=Full Time, S=Part Time, C=Contractual")]
    [MaxLength(10)]
    public string? WorkType { get; set; }

    public Guid? DisciplineId { get; set; }

    [Precision(2, 0)]
    public decimal Discipline { get; set; }

    [Precision(5, 0)]
    public decimal? Semester { get; set; }

    [Precision(6, 4)]
    public decimal? Credit { get; set; }
}