using AACSB.WebApi.Domain.ReportGenerator.Function;

namespace AACSB.WebApi.Application.ReportGenerator.Reports;

public class GetTableA31ByDisciplineRequest : IRequest<ICollection<TableA31>>
{
    public int[] Semester { get; set; }
    public string Discipline { get; set; }
}

public class GetTableA31ByDisciplineRequestHandler : IRequestHandler<GetTableA31ByDisciplineRequest, ICollection<TableA31>>
{
    private readonly IDapperRepository _repository;
    public GetTableA31ByDisciplineRequestHandler(IDapperRepository repository) => _repository = repository;
    public async Task<ICollection<TableA31>> Handle(GetTableA31ByDisciplineRequest request, CancellationToken cancellationToken)
    {
        string sql = "SELECT d.*, r.[Responsibilities] " +
                     "FROM [ReportGenerator].[F_GetTeacherDiscipline](@Semester) d " +
                     "LEFT JOIN [ReportGenerator].[F_GetTeacherResponsibilities](@Semester) r ON d.[TeacherId] = r.[TeacherId] " +
                     "WHERE [Discipline] = @Discipline";

        string semester = string.Empty;
        foreach (int s in request.Semester)
        {
            if (s.ToString().Length != 4)
            {
                throw new ArgumentException($"Semester {s.ToString()} Format Error.");
            }

            semester += string.Concat(s.ToString(), ",");
        }

        semester = semester[..^1];
        var sqlParams = new { semester, request.Discipline };

        var tableA31 = await _repository.QueryAsync<TableA31>(sql, sqlParams, cancellationToken: cancellationToken);
        _ = tableA31 ?? throw new NotFoundException("TableA31 Not Found.");

        return tableA31.ToList();
    }

}