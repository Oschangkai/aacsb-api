using AACSB.WebApi.Domain.ReportGenerator.Function;

namespace AACSB.WebApi.Application.ReportGenerator;

public class GetTableA31ByDisciplineRequest : IRequest<ICollection<TableA31>>
{
    public string Semester { get; set; }
    public string Discipline { get; set; }
}

public class GetTableA31ByDisciplineRequestHandler : IRequestHandler<GetTableA31ByDisciplineRequest, ICollection<TableA31>>
{
    private readonly IDapperRepository _repository;
    public GetTableA31ByDisciplineRequestHandler(IDapperRepository repository) => _repository = repository;
    public async Task<ICollection<TableA31>> Handle(GetTableA31ByDisciplineRequest request, CancellationToken cancellationToken)
    {
        var tableA31 = await _repository.QueryAsync<TableA31>(
            $"SELECT * FROM [ReportGenerator].[F_GetTeacherDiscipline]({request.Semester}) WHERE [Discipline] = '{request.Discipline}'", cancellationToken: cancellationToken);
        _ = tableA31 ?? throw new NotFoundException("TableA31 Not Found.");

        return tableA31.ToList();
    }

}