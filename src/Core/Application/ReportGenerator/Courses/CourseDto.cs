namespace AACSB.WebApi.Application.ReportGenerator.Course;

public class CourseDto : IDto
{
    public decimal Semester { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public decimal Credit { get; set; }
    public string Time { get; set; }
}