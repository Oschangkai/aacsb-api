using AACSB.WebApi.Domain.ReportGenerator.View;
using Mapster;

namespace AACSB.WebApi.Application.ReportGenerator.Courses;

public class GetNullDisciplineCoursesRequest : IRequest<List<ACourseDto>>
{
    public string AcademicYear { get; set; }
    // public GetNullDisciplineCoursesRequest(string academicYear) => AcademicYear = academicYear;
}

public class GetNullDisciplineCoursesRequestValidator : CustomValidator<GetNullDisciplineCoursesRequest>
{
    public GetNullDisciplineCoursesRequestValidator() =>
        RuleFor(c => c.AcademicYear)
            .NotEmpty();
}

public class GetNullDisciplineCoursesRequestHandler : IRequestHandler<GetNullDisciplineCoursesRequest, List<ACourseDto>>
{
    private readonly IDapperRepository _repository;
    public GetNullDisciplineCoursesRequestHandler(IDapperRepository repository) => _repository = repository;

    public async Task<List<ACourseDto>> Handle(GetNullDisciplineCoursesRequest request, CancellationToken cancellationToken)
    {
        const string sql = "SELECT [Course], [CourseCode], [Teacher], [TeacherEnglishName], [CourseDepartment], [Semester]" +
            " FROM [ReportGenerator].[V_Table_A31_Course]" +
            " WHERE ([Semester] = @StartSemester OR [Semester] = @EndSemester)" +
            " AND [Discipline] IS NULL";
        (string startSemester, string endSemester) = (request.AcademicYear + "1", request.AcademicYear + "2");
        object sqlParams = new { startSemester, endSemester };

        var nullDisciplineCourses = await _repository.QueryAsync<TableA31Course>(sql, sqlParams, cancellationToken: cancellationToken);
        _ = nullDisciplineCourses ?? throw new NotFoundException("Not Found.");
        return nullDisciplineCourses.Adapt<List<ACourseDto>>();
    }
}