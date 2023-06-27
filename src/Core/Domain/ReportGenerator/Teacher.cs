using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AACSB.WebApi.Domain.ReportGenerator;

[Table("Teachers", Schema="ReportGenerator")]
public class Teacher : AuditableEntity, IAggregateRoot
{
    [MaxLength(80)]
    public string Name { get; set; }

    [MaxLength(200)]
    public string? NameInNtustCourse { get; set; }

    public Guid? QualificationId { get; set; }
    public Qualification? Qualification { get; set; }

    [Comment("P=Full Time, S=Part Time, C=Contractual")]
    [MaxLength(10)]
    public string? WorkTypeAbbr { get; set; }

    [MaxLength(200)]
    public string? EnglishName { get; set; }

    [MaxLength(200)]
    public string? EnglishNameInNtustCourse { get; set; }

    [MaxLength(10)]
    public string? Degree { get; set; }

    [Precision(4, 0)]
    public decimal? DegreeYear { get; set; }

    public Guid? DepartmentId { get; set; }
    public Department? Department { get; set; }

    [Comment("MT/RES/... 可以為多值")]
    [MaxLength(100)]
    public string? Responsibilities { get; set; }

    [Comment("Email")]
    [MaxLength(80)]
    public string? Email { get; set; }

    [Comment("職稱")]
    [MaxLength(80)]
    public string? Title { get; set; }

    [Comment("離職日期")]
    public DateTime? ResignDate { get; set; }

    [Comment("學術研究相關")]
    public ICollection<Research>? Research { get; set; }

    [Comment("專業領域相關")]
    public ICollection<Professional>? Professional { get; set; }

    public Guid? ImportSignatureId { get; set; }
    public ImportSignature? ImportSignature { get; set; }

    public ICollection<CourseTeacher>? Courses { get; set; }
}