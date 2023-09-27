namespace AACSB.WebApi.Application.ReportGenerator.Reports;

public class GetSupportingHeadcountRequest : IRequest<ICollection<SupportingHeadcount>>
{
    public int[] Semester { get; set; }
}

public class GetSupportingHeadcountRequestHandler : IRequestHandler<GetSupportingHeadcountRequest, ICollection<SupportingHeadcount>>
{
    private readonly IDapperRepository _repository;
    public GetSupportingHeadcountRequestHandler(IDapperRepository repository) => _repository = repository;
    public async Task<ICollection<SupportingHeadcount>> Handle(GetSupportingHeadcountRequest request, CancellationToken cancellationToken)
    {
        foreach (int s in request.Semester)
        {
            if (s.ToString().Length != 4)
            {
                throw new ArgumentException($"Semester {s.ToString()} Format Error.");
            }
        }

        string sql = $"SELECT d.[EnglishName] AS [Department], COUNT(*) AS Headcount " +
                           $"FROM [ReportGenerator].[Teachers] t " +
                           $"LEFT JOIN [ReportGenerator].[Departments] d ON t.[DepartmentId] = d.Id " +
                           $"WHERE t.Id IN ( " +
                                $"SELECT DISTINCT [TeacherId] " +
                                $"FROM ReportGenerator.V_Table_A31_Course c " +
                                $"WHERE [Semester] IN (" + string.Join(",", request.Semester) + $") " +
                                $"AND c.WorkType <> 'P') " +
                           $"GROUP BY d.EnglishName";

        var supportingHeadcount = await _repository.QueryAsync<SupportingHeadcount>(sql, cancellationToken: cancellationToken);
        _ = supportingHeadcount ?? throw new NotFoundException("SupportingHeadcount Not Found.");

        return supportingHeadcount.ToList();
    }

}

public class SupportingHeadcount
{
    public string Department { get; set; }
    public int Headcount { get; set; }
}