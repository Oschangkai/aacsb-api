using AACSB.WebApi.Domain.ReportGenerator.View;
using Mapster;

namespace AACSB.WebApi.Application.ReportGenerator.Teachers;

public class GetMissingDataTeachersRequest : IRequest<List<ATeacherDto>>
{
    public string Column { get; set; }
    public string AcademicYear { get; set; }
}

public class GetMissingDataTeachersRequestValidator : CustomValidator<GetMissingDataTeachersRequest>
{
    public GetMissingDataTeachersRequestValidator()
    {
        var conditions = new List<string>() { "degree", "responsibility", "qualification", "worktype" };

        RuleFor(c => c.Column)
            .Must(x => conditions.Contains(x.ToLower()))
            .WithMessage("Column only accepts: " + string.Join(",", conditions));

        RuleFor(c => c.AcademicYear)
            .NotEmpty();
    }
}

public class GetMissingDataTeachersRequestHandler : IRequestHandler<GetMissingDataTeachersRequest, List<ATeacherDto>>
{
    private readonly IDapperRepository _repository;
    public GetMissingDataTeachersRequestHandler(IDapperRepository repository) => _repository = repository;

    public async Task<List<ATeacherDto>> Handle(GetMissingDataTeachersRequest request, CancellationToken cancellationToken)
    {
        string sql = "SELECT DISTINCT [Teacher], [TeacherEnglishName], [Degree], [DegreeYear], [Responsibilities], [Qualification], [TeacherId]" +
                     " FROM [ReportGenerator].[V_Table_A31_Course]" +
                     " WHERE ([Semester] = @StartSemester OR [Semester] = @EndSemester)";
        sql += request.Column.ToLower() switch
        {
            "degree" => " AND ([Degree] IS NULL OR [DegreeYear] IS NULL)",
            "responsibility" => " AND [Responsibilities] IS NULL",
            "qualification" => " AND [Qualification] IS NULL",
            "worktype" => " AND [WorkType] IS NULL",
            _ => throw new ArgumentException("Column only accepts: Degree, Responsibility, Qualification")
        };

        (string startSemester, string endSemester) = (request.AcademicYear + "1", request.AcademicYear + "2");
        object sqlParams = new { startSemester, endSemester };

        var missingDataTeachers = await _repository.QueryAsync<TableA31Course>(sql, sqlParams, cancellationToken: cancellationToken);
        _ = missingDataTeachers ?? throw new NotFoundException("Not Found.");
        return missingDataTeachers.Adapt<List<ATeacherDto>>();
    }
}