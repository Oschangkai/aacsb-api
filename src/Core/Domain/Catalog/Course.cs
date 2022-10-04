using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace AACSB.WebApi.Domain.Catalog;

public class Course : AuditableEntity, IAggregateRoot
{
    [Precision(2, 0)]
    public decimal? DisciplineCode { get; private set; }
    [Precision(5, 0)]
    public decimal Semester { get; private set; }
    [MaxLength(50)]
    public string CourseCode { get; private set; }
    [MaxLength(200)]
    public string CourseName { get; private set; }
    [MaxLength(500)]
    public string CourseNameEng { get; private set; }
    [MaxLength(80)]
    public string TeacherName { get; private set; }
    [MaxLength(200)]
    public string TeacherEnglishName { get; private set; }
    [Precision(6, 4)]
    public decimal CourseCredit { get; private set; }
    [Comment("必修/選修")]
    public bool CourseRequired { get; private set; }
    [Comment("全半學年")]
    public bool CourseYear { get; private set; }
    [Comment("節次，M1, T6, W3，以逗點分隔")]
    [MaxLength(500)]
    public string CourseTime { get; private set; }

    public Guid ImportSignatureId { get; private set; }
    public ImportSignature ImportSignature { get; private set; }
    public Discipline Discipline { get; private set; }

    public Course()
    {
        // Only needed for working with dapper (See GetProductViaDapperRequest)
        // If you're not using dapper it's better to remove this constructor.
    }

    public Course(int disciplineCode, decimal semester,
        string courseCode, string courseName, string courseNameEng, string teacherName, string teacherEnglishName,
        decimal courseCredit, bool courseRequired, bool courseYear, string courseTime, Guid importSignatureId)
    {
        DisciplineCode = disciplineCode;
        Semester = semester;
        CourseCode = courseCode;
        CourseName = courseName;
        CourseNameEng = courseNameEng;
        TeacherName = teacherName;
        TeacherEnglishName = teacherEnglishName;
        CourseCredit = courseCredit;
        CourseRequired = courseRequired;
        CourseYear = courseYear;
        CourseTime = courseTime;
        ImportSignatureId = importSignatureId;
    }
}