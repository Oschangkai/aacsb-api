namespace AACSB.WebApi.Application.ReportGenerator.Courses;

public class GetSemesterRequest : IRequest<int[]>
{
}

public class GetSemesterRequestHandler : IRequestHandler<GetSemesterRequest, int[]>
{
    private readonly IDapperRepository _repository;
    public GetSemesterRequestHandler(IDapperRepository repository) => _repository = repository;
    public async Task<int[]> Handle(GetSemesterRequest request, CancellationToken cancellationToken)
    {
        var academicYear = await _repository.QueryAsync<int>(
            $"SELECT DISTINCT CAST([Semester] AS DECIMAL(4,0)) FROM [ReportGenerator].[Courses]", cancellationToken: cancellationToken);
        _ = academicYear ?? throw new NotFoundException("AcademicYear Not Found.");

        return academicYear.ToArray();
    }

}