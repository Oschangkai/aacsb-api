using AACSB.WebApi.Domain.ReportGenerator.Function;

namespace AACSB.WebApi.Application.ReportGenerator.Reports;

public class GetTableA31Request : IRequest<ICollection<TableA31>>
{
    public string Semester { get; set; }
}

public class GetTableA31RequestHandler : IRequestHandler<GetTableA31Request, ICollection<TableA31>>
{
    private readonly IDapperRepository _repository;
    public GetTableA31RequestHandler(IDapperRepository repository) => _repository = repository;
    public async Task<ICollection<TableA31>> Handle(GetTableA31Request request, CancellationToken cancellationToken)
    {
        string sql = $"SELECT d.*, r.[Responsibilities] " +
                     $"FROM [ReportGenerator].[F_GetTeacherDiscipline](@Semester) d " +
                     $"LEFT JOIN [ReportGenerator].[F_GetTeacherResponsibilities](@Semester) r ON d.[TeacherId] = r.[TeacherId]";
        var sqlParams = new { request.Semester };

        var tableA31 = await _repository.QueryAsync<TableA31>(sql, sqlParams, cancellationToken: cancellationToken);
        _ = tableA31 ?? throw new NotFoundException("TableA31 Not Found.");

        return tableA31.ToList();
    }

}