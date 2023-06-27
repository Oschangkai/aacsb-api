namespace AACSB.WebApi.Application.ReportGenerator.Courses;

public class ACourseDto
{
    public string? Course { get; set; }
    public string? CourseCode { get; set; }
    public string Teacher { get; set; }
    public string? TeacherEnglishName { get; set; }
    public string? CourseDepartment { get; set; }
    public decimal Semester { get; set; }
    public Guid CourseId { get; set; }
    public Guid? TeacherId { get; set; }
}