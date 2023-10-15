using AACSB.WebApi.Domain.ReportGenerator.Function;

namespace AACSB.WebApi.Application.ReportGenerator.Reports;

public class GetTeacherResumeRequest : IRequest<ICollection<TeacherResume>>
{
    public int[] Semester { get; set; }
}

public class GetTeacherResumeRequestHandler : IRequestHandler<GetTeacherResumeRequest, ICollection<TeacherResume>>
{
    private readonly IDapperRepository _repository;
    public GetTeacherResumeRequestHandler(IDapperRepository repository) => _repository = repository;

    public async Task<ICollection<TeacherResume>> Handle(GetTeacherResumeRequest request, CancellationToken cancellationToken)
    {
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

        var teacherResume = new List<TeacherResume>();

        const string getTeacherIdListSql =
            "SELECT DISTINCT TeacherId\n" +
            "FROM ReportGenerator.V_Table_A31_Course\n" +
            "WHERE [Semester] IN (SELECT value FROM STRING_SPLIT(@Semester, ',')) AND [WorkType]='P'";
        var teacherIdList = await _repository.QueryAsync<TeacherIdList>(getTeacherIdListSql, new { semester }, cancellationToken: cancellationToken);
        _ = teacherIdList ?? throw new NotFoundException("TeacherIdList Empty.");

        const string getCourseListSql =
            "SELECT CourseCode AS Code, Course AS Name, CourseEnglish AS EnglishName, CourseTime AS Time\n" +
            ", TeacherId, Teacher, Semester\n" +
            "FROM [ReportGenerator].[V_Table_A31_Course] c\n" +
            "WHERE [Semester] IN (SELECT value FROM STRING_SPLIT(@Semester, ','))";
        var courseList = await _repository.QueryAsync<TeacherResumeCourse>(getCourseListSql, new { semester }, cancellationToken: cancellationToken);

        const string getResearchListSql =
            "SELECT r.TeacherId, OtherAuthors AS Authors, COALESCE(Year, YearEnd, YearStart) AS Year,\n" +
                "r.Title, COALESCE(JournalsName, Seminar) AS AppearedIn,\n" +
                "r.Volume, r.Issue, NULLIF(CONCAT_WS('-', PageStart, PageEnd), '') AS Page, r.JournalsClass AS Class,\n" +
                "STRING_AGG(RT.Code, ',') AS ResearchTypeCode\n" +
            "FROM ReportGenerator.Research r\n" +
                "LEFT JOIN ReportGenerator.ResearchResearchType rrt on r.Id = rrt.ResearchId\n" +
                "LEFT JOIN ReportGenerator.ResearchType rt on rrt.ResearchTypeId = rt.Id\n" +
            "WHERE COALESCE(r.Year, r.YearEnd, r.YearStart) >= YEAR(GETDATE()) - 5 \n" +
            "GROUP BY r.TeacherId, r.OtherAuthors, r.Year, r.YearEnd, r.YearStart, r.Month, r.MonthEnd, r.MonthStart, r.Title, r.JournalsName, r.Seminar, r.JournalsClass, r.Volume, r.Issue, r.PageStart, r.PageEnd\n" +
            "ORDER BY COALESCE(r.Year, r.YearEnd, r.YearStart) DESC, COALESCE(Month, MonthEnd, MonthStart) DESC";
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