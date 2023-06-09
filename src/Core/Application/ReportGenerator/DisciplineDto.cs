namespace AACSB.WebApi.Application.ReportGenerator;

public class DisciplineDto : IDto
{
    public string Name { get; set; } = default!;
    public string Code { get; set; } = default!;
    public Guid Id { get; set; } = default!;
}