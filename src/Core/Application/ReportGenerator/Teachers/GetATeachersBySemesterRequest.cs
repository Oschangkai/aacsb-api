using Mapster;

namespace AACSB.WebApi.Application.ReportGenerator.Teachers;

public class GetATeachersBySemesterRequest : IRequest<List<ASimpleTeacherDto>>
{
    public string AcademicYear { get; set; }
}

public class GetATeachersBySemesterRequestValidator : CustomValidator<GetATeachersBySemesterRequest>
{
    public GetATeachersBySemesterRequestValidator() =>
        RuleFor(c => c.AcademicYear)
            .NotEmpty();
}

public class GetATeachersBySemesterRequestHandler : IRequestHandler<GetATeachersBySemesterRequest, List<ASimpleTeacherDto>>
{
    private readonly IDapperRepository _repository;
    public GetATeachersBySemesterRequestHandler(IDapperRepository repository) => _repository = repository;

    public async Task<List<ASimpleTeacherDto>> Handle(GetATeachersBySemesterRequest request, CancellationToken cancellationToken)
    {
        string sql = "SELECT DISTINCT [Teacher], [TeacherEnglishName]" +
                     " FROM [ReportGenerator].[V_Table_A31_Course]" +
                     " WHERE ([Semester] = @StartSemester OR [Semester] = @EndSemester)";

        (string startSemester, string endSemester) = (request.AcademicYear + "1", request.AcademicYear + "2");
        object sqlParams = new { startSemester, endSemester };

        var missingDataTeachers = await _repository.QueryAsync<ASimpleTeacherDto>(sql, sqlParams, cancellationToken: cancellationToken);
        _ = missingDataTeachers ?? throw new NotFoundException("Not Found.");
        return missingDataTeachers.Adapt<List<ASimpleTeacherDto>>();
    }
}