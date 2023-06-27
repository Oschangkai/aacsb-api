using AACSB.WebApi.Domain.ReportGenerator.View;
using Mapster;

namespace AACSB.WebApi.Application.ReportGenerator.Courses;

public class GetTeacherCoursesRequest : IRequest<List<ACourseDto>>
{
    public string AcademicYear { get; set; }
    public string TeacherName { get; set; }
}

public class GetTeacherCoursesRequestValidator : CustomValidator<GetTeacherCoursesRequest>
{
    public GetTeacherCoursesRequestValidator()
    {
        RuleFor(x => x.AcademicYear).NotEmpty();
        RuleFor(x => x.TeacherName).NotEmpty();
    }
}

public class GetTeacherCoursesRequestHandler : IRequestHandler<GetTeacherCoursesRequest, List<ACourseDto>>
{
    private readonly IDapperRepository _repository;
    public GetTeacherCoursesRequestHandler(IDapperRepository repository) => _repository = repository;

    public async Task<List<ACourseDto>> Handle(GetTeacherCoursesRequest request, CancellationToken cancellationToken)
    {
        const string sql = "SELECT [Course], [CourseCode], [Teacher], [TeacherEnglishName], [CourseDepartment], [Semester], [CourseId], [TeacherId]" +
            " FROM [ReportGenerator].[V_Table_A31_Course]" +
            " WHERE ([Semester] = @StartSemester OR [Semester] = @EndSemester) AND [Teacher] LIKE '%' + @TeacherName + '%'" ;
        var (startSemester, endSemester) = (request.AcademicYear + "1", request.AcademicYear + "2");
        object sqlParams = new { startSemester, endSemester, request.TeacherName };

        var teacherCourses = await _repository.QueryAsync<TableA31Course>(sql, sqlParams, cancellationToken: cancellationToken);
        _ = teacherCourses ?? throw new NotFoundException("Not Found.");
        return teacherCourses.Adapt<List<ACourseDto>>();
    }
}