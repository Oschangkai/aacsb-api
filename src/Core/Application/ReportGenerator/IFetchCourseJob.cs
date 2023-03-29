namespace AACSB.WebApi.Application.ReportGenerator;

public interface IFetchCourseJob : IScopedService
{
    Task FetchAsync(int year, int semester, string[] department, CancellationToken cancellationToken);
}