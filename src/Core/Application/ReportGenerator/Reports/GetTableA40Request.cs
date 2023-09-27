using AACSB.WebApi.Domain.ReportGenerator.Function;

namespace AACSB.WebApi.Application.ReportGenerator.Reports;

public class GetTableA40Request : IRequest<ICollection<TableA40>>
{
    public int[] Semester { get; set; }
}

public class GetTableA40RequestHandler : IRequestHandler<GetTableA40Request, ICollection<TableA40>>
{
    private readonly IDapperRepository _repository;
    public GetTableA40RequestHandler(IDapperRepository repository) => _repository = repository;
    public async Task<ICollection<TableA40>> Handle(GetTableA40Request request, CancellationToken cancellationToken)
    {
        foreach (int s in request.Semester)
        {
            if (s.ToString().Length != 4)
            {
                throw new ArgumentException($"Semester {s.ToString()} Format Error.");
            }
        }

        string sql = $"SELECT d.[EnglishName] AS [Department], [CourseDegree], [WorkType], ROUND(SUM([Credit]), 2) AS [Credit] " +
                           $"FROM [ReportGenerator].[V_Table_A31_Course] c " +
                           $"LEFT JOIN [ReportGenerator].[Departments] d ON c.[CourseDepartmentId] = d.Id " +
                           $"WHERE [Semester] IN (" + string.Join(",", request.Semester.Select(s => string.Concat("'", s, "'"))) + $") " +
                           $"GROUP BY d.EnglishName, CourseDegree, WorkType";

        var tableA40 = await _repository.QueryAsync<TableA40>(sql, cancellationToken: cancellationToken);
        _ = tableA40 ?? throw new NotFoundException("TableA40 Not Found.");

        return tableA40.ToList();
    }

}