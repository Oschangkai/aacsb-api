using AACSB.WebApi.Domain.ReportGenerator.Function;
using System;

namespace AACSB.WebApi.Application.ReportGenerator.Reports;

public class GetTableA31Request : IRequest<ICollection<TableA31>>
{
    public int[] Semester { get; set; }
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
        var sqlParams = new { semester };

        var tableA31 = await _repository.QueryAsync<TableA31>(sql, sqlParams, cancellationToken: cancellationToken);
        _ = tableA31 ?? throw new NotFoundException("TableA31 Not Found.");

        return tableA31.ToList();
    }

}