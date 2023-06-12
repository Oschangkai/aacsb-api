namespace AACSB.WebApi.Application.ReportGenerator;

public class DepartmentDto : IDto
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? EnglishName { get; set; }

    public string? Abbreviation { get; set; }
}