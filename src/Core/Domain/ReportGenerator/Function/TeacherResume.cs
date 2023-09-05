using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
namespace AACSB.WebApi.Domain.ReportGenerator.Function;

public class TeacherResume
{
    // Teacher Information
    [MaxLength(200)]
    public string Name { get; set; }

    [MaxLength(200)]
    public string? EnglishName { get; set; }

    [MaxLength(10)]
    public string? Degree { get; set; }

    public short? DegreeYear { get; set; }

    [MaxLength(80)]
    public string? Department { get; set; }

    [MaxLength(10)]
    public string? Qualification { get; set; }

    // Teacher Researches
    public List<TeacherResumeResearch>? Research { get; set; }

    // Teacher Courses
    public List<TeacherResumeCourse>? Course { get; set; }
}

public class TeacherResumeResearch
{
    public string Value { get; set; }

    public string Type { get; set; }
    public Guid TeacherId { get; set; }
}

public class TeacherResumeCourse
{
    [MaxLength(50)]
    public string Code { get; set; }

    [MaxLength(200)]
    public string Name { get; set; }

    [MaxLength(500)]
    public string EnglishName { get; set; }

    [Comment("節次，M1, T6, W3，以逗點分隔")]
    [MaxLength(256)]
    public string? Time { get; set; }

    public short Semester { get; set; }

    [MaxLength(200)]
    public string Teacher { get; set; }

    public Guid? TeacherId { get; set; }
}
