namespace AACSB.WebApi.Application.ReportGenerator;

public class CourseDto
{
    public decimal Semester { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string EnglishName { get; set; }
    public decimal Credit { get; set; }
    public bool Required { get; set; }
    public bool Year { get; set; }
    public string Time { get; set; }
}