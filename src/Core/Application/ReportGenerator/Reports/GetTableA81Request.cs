using AACSB.WebApi.Domain.ReportGenerator.Function;

namespace AACSB.WebApi.Application.ReportGenerator.Reports;

public class GetTableA81Request : IRequest<ICollection<TableA81>>
{
    public int[] Semester { get; set; }
}

public class GetTableA81RequestHandler : IRequestHandler<GetTableA81Request, ICollection<TableA81>>
{
    private readonly IDapperRepository _repository;
    public GetTableA81RequestHandler(IDapperRepository repository) => _repository = repository;
    public async Task<ICollection<TableA81>> Handle(GetTableA81Request request, CancellationToken cancellationToken)
    {
        string sql = $"SELECT *" +
                     $"FROM [ReportGenerator].[F_GetTeacherResearchCount](@Semester)";

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

        var tableA81 = await _repository.QueryAsync<TableA81>(sql, sqlParams, cancellationToken: cancellationToken);
        _ = tableA81 ?? throw new NotFoundException("TableA81 Not Found.");

        return tableA81.ToList();
    }

}