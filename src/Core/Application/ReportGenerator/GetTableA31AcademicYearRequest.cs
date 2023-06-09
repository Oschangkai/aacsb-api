namespace AACSB.WebApi.Application.ReportGenerator;

public class GetTableA31AcademicYearRequest : IRequest<int[]>
{
}

public class GetTableA31AcademicYearRequestHandler : IRequestHandler<GetTableA31AcademicYearRequest, int[]>
{
    private readonly IDapperRepository _repository;
    public GetTableA31AcademicYearRequestHandler(IDapperRepository repository) => _repository = repository;
    public async Task<int[]> Handle(GetTableA31AcademicYearRequest request, CancellationToken cancellationToken)
    {
        var academicYear = await _repository.QueryAsync<int>(
            $"SELECT DISTINCT CAST(ROUND([Semester] / 10, 0) AS DECIMAL(3,0)) FROM [ReportGenerator].[V_Table_A31_Course]", cancellationToken: cancellationToken);
        _ = academicYear ?? throw new NotFoundException("AcademicYear Not Found.");

        return academicYear.ToArray();
    }

}