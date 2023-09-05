using AACSB.WebApi.Domain.ReportGenerator.Function;

namespace AACSB.WebApi.Application.ReportGenerator.Reports;

public class GetTeacherResumeRequest : IRequest<ICollection<TeacherResume>>
{
    public string AcademicYear { get; set; }
}

public class GetTeacherResumeRequestHandler : IRequestHandler<GetTeacherResumeRequest, ICollection<TeacherResume>>
{
    private readonly IDapperRepository _repository;
    public GetTeacherResumeRequestHandler(IDapperRepository repository) => _repository = repository;

    public async Task<ICollection<TeacherResume>> Handle(GetTeacherResumeRequest request, CancellationToken cancellationToken)
    {
        var teacherResume = new List<TeacherResume>();

        const string getTeacherIdListSql =
            "SELECT DISTINCT TeacherId\n" +
            "FROM ReportGenerator.V_Table_A31_Course\n" +
            "WHERE Semester IN (@AcademicYear+'1', @AcademicYear+'2')";
        var teacherIdList = await _repository.QueryAsync<TeacherIdList>(getTeacherIdListSql, new { request.AcademicYear }, cancellationToken: cancellationToken);
        _ = teacherIdList ?? throw new NotFoundException("TeacherIdList Empty.");

        const string getCourseListSql =
            "SELECT CourseCode AS Code, Course AS Name, CourseEnglish AS EnglishName, CourseTime AS Time\n" +
            ", TeacherId, Teacher, Semester\n" +
            "FROM [ReportGenerator].[V_Table_A31_Course] c\n" +
            "WHERE Semester IN (@AcademicYear+'1', @AcademicYear+'2')";
        var courseList = await _repository.QueryAsync<TeacherResumeCourse>(getCourseListSql, new { request.AcademicYear }, cancellationToken: cancellationToken);

        const string getResearchListSql =
            "SELECT CONCAT_WS(', ',\n" +
                "r.OtherAuthors, COALESCE(NULLIF(r.Year, ''), r.YearStart), r.Title,\n" +
                "r.JournalsName, r.JournalsClass, r.Volume,\n" +
                "IIF(PageStart IS NOT NULL AND PageStart <> 0, CONCAT(PageStart, '-', PageEnd), NULL)\n" +
            ") AS Value, r.Type, r.TeacherId\n" +
            "FROM ReportGenerator.Research r";
        var researchList = await _repository.QueryAsync<TeacherResumeResearch>(getResearchListSql, cancellationToken: cancellationToken);

        foreach (var teacherId in teacherIdList.Select(x => x.TeacherId))
        {
            const string teacherInfoSql =
                "SELECT t.Name, t.EnglishName, t.DegreeYear, t.Degree, d.EnglishName AS Department, q.Abbreviation AS Qualification\n" +
                "FROM ReportGenerator.Teachers t\n" +
                "LEFT JOIN ReportGenerator.Departments d ON t.DepartmentId = d.Id\n" +
                "LEFT JOIN ReportGenerator.Qualifications q ON t.QualificationId = q.Id\n" +
                "WHERE t.Id = @TeacherId";
            var teacherInfo = await _repository.QueryFirstOrDefaultAsync<TeacherInfo>(teacherInfoSql, new { TeacherId = teacherId }, cancellationToken: cancellationToken);
            _ = teacherInfo ?? throw new NotFoundException("TeacherInfo Not Found.");

            teacherResume.Add(new TeacherResume()
            {
                Name = teacherInfo.Name,
                EnglishName = teacherInfo.EnglishName,
                Degree = teacherInfo.Degree,
                DegreeYear = teacherInfo.DegreeYear,
                Department = teacherInfo.Department,
                Qualification = teacherInfo.Qualification,
                Research = (from r in researchList where r.TeacherId == teacherId select r).ToList(),
                Course = (from c in courseList where c.TeacherId == teacherId select c).ToList()
            });
        }

        return teacherResume;
    }
}

internal class TeacherIdList
{
    public Guid TeacherId { get; set; }
}

internal class TeacherInfo
{
    public string Name { get; set; }
    public string EnglishName { get; set; }
    public short DegreeYear { get; set; }
    public string Degree { get; set; }
    public string Department { get; set; }
    public string Qualification { get; set; }
}