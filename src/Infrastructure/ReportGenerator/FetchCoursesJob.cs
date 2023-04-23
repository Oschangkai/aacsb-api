using System.Text.Json;
using System.Text.RegularExpressions;
using AACSB.WebApi.Application.Common.Interfaces;
using AACSB.WebApi.Application.Common.Persistence;
using AACSB.WebApi.Application.ReportGenerator;
using AACSB.WebApi.Domain.ReportGenerator;
using AACSB.WebApi.Infrastructure.ReportGenerator.Request;
using AACSB.WebApi.Infrastructure.ReportGenerator.Request.Model;
using AACSB.WebApi.Infrastructure.ReportGenerator.Specifications;
using AACSB.WebApi.Shared.Notifications;
using Hangfire;
using Hangfire.Console.Extensions;
using Hangfire.Console.Progress;
using Hangfire.Server;
using Microsoft.Extensions.Logging;

namespace AACSB.WebApi.Infrastructure.ReportGenerator;

public class FetchCoursesJob : IFetchCourseJob
{
    private readonly ILogger<FetchCoursesJob> _logger;
    private readonly IRepository<Course> _courseRepo;
    private readonly IRepository<Teacher> _teacherRepo;
    private readonly IReadRepository<Department> _departmentRepo;
    private readonly IRepository<ImportSignature> _importSignatureRepo;
    private readonly IProgressBarFactory _progressBar;
    private readonly PerformingContext _performingContext;
    private readonly INotificationSender _notifications;
    private readonly ICurrentUser _currentUser;
    private readonly IProgressBar _progress;
    public FetchCoursesJob(
        ILogger<FetchCoursesJob> logger,
        IRepository<Course> courseRepo,
        IRepository<Teacher> teacherRepo,
        IReadRepository<Department> departmentRepo,
        IRepository<ImportSignature> importSignatureRepo,
        IProgressBarFactory progressBar,
        PerformingContext performingContext,
        INotificationSender notifications,
        ICurrentUser currentUser)
    {
        _logger = logger;
        _courseRepo = courseRepo;
        _teacherRepo = teacherRepo;
        _departmentRepo = departmentRepo;
        _importSignatureRepo = importSignatureRepo;
        _progressBar = progressBar;
        _performingContext = performingContext;
        _notifications = notifications;
        _currentUser = currentUser;
        _progress = _progressBar.Create();
    }

    private async Task NotifyAsync(string message, int progress, CancellationToken cancellationToken)
    {
        _progress.SetValue(progress);
        await _notifications.SendToUserAsync(
            new JobNotification()
            {
                JobId = _performingContext.BackgroundJob.Id,
                Message = message,
                Progress = progress
            },
            _currentUser.GetUserId().ToString() ?? string.Empty,
            cancellationToken);
    }

    [Queue("notdefault")]
    public async Task FetchAsync(int year, int semester, string[] departments, CancellationToken cancellationToken)
    {
        // Task1: If department not provided, fetch all departments related to 管院
        if (departments.Length <= 0)
        {
            departments = (await _departmentRepo.ListAsync(new DepartmentAbbrListSpec(), cancellationToken)).ToArray();
        }

        // Task2: Create Import Signature
        var importSignature = new ImportSignature()
        {
            Name = $"{DateTime.UtcNow:yyyyMMdd-HHmmss}-auto",
            Description = $"Auto generated. {DateTime.UtcNow:O}"
        };
        var signature = await _importSignatureRepo.AddAsync(importSignature, cancellationToken);
        if (signature == null) throw new ArgumentNullException(nameof(signature));

        // Task3: Fetch courses
        _logger.LogInformation($"{departments.Length} departments found: {string.Join(", ", departments)}");
        await NotifyAsync($"Fetch courses started: {year}{semester}, {departments} with signature {importSignature.Name}", 0, cancellationToken);

        var request = new CourseQueryRequest();
        var courseList = new List<Course>();

        foreach (var dept in departments)
        {
            var department = await _departmentRepo.FirstOrDefaultAsync(new GetDepartmentByAbbrSpec(dept), cancellationToken);
            _logger.LogInformation($"Fetch courses: {year}{semester}, {dept} with signature {importSignature.Name}");

            // Task3.1: Fetch Chinese and English course
            var (chineseCourseString, englishCourseString) = await request.GetCourseByDepartment($"{year}{semester}", dept, cancellationToken);
            var chineseCourses = JsonSerializer.Deserialize<List<CourseResponse>>(await chineseCourseString);
            var englishCourses = JsonSerializer.Deserialize<List<CourseResponse>>(await englishCourseString);

            // Task3.2: Merge Chinese and English course
            var courses =
                from cc in chineseCourses
                    join ec in englishCourses on cc.CourseNo equals ec.CourseNo
                    select new
                    {
                        Semester = Convert.ToDecimal($"{year}{semester}"),
                        Code = cc.CourseNo,
                        Name = cc.CourseName,
                        EnglishName = ec.CourseName,
                        DepartmentId = department.Id,
                        Credit = Convert.ToDecimal(cc.CreditPoint),
                        Required = cc.RequireOption == "R", // Required = R, Not required = E
                        Year = cc.AllYear == "F", // Full Year = F, Half Year = H
                        Time = cc.CourseTimes,
                        ClassRoomNo = cc.ClassRoomNo,
                        Contents = cc.Contents,
                        ImportSignatureId = signature.Id,
                        Teachers = new List<Teacher>()
                        {
                            new Teacher() { Name = cc.CourseTeacher.Trim(), EnglishName = ec.CourseTeacher.Trim(), EnglishNameInNtustCourse = ec.CourseTeacher.Trim() }
                        },
                    };
            /*
             * Task3.3: Split teacher name & Create teacher if not exist
             * If teacher is multiple, split by comma
             * Create teacher if not exist otherwise link to existing teacher
             */

            var cl = new List<Course>();
            foreach (var c in courses)
            {
                var course = new Course()
                {
                    Semester = c.Semester,
                    Code = c.Code,
                    Name = c.Name,
                    EnglishName = c.EnglishName,
                    DepartmentId = c.DepartmentId,
                    Credit = c.Credit,
                    Required = c.Required,
                    Year = c.Year,
                    Time = c.Time,
                    ClassRoomNo = c.ClassRoomNo,
                    Contents = c.Contents,
                    ImportSignatureId = signature.Id,
                };
                course.Teachers = await ProcessTeacherString(c.Teachers!, signature);
                cl.Add(course);
            }

            _logger.LogInformation($"{year}{semester} {dept} has {cl.Count()} courses.");
            courseList.AddRange(cl);
        }

        // Task4: Insert courses
        _logger.LogInformation($"Semester {year}{semester} has {courseList.Count} courses with signature {importSignature.Name} will be inserted.");
        await _courseRepo.AddRangeAsync(courseList, cancellationToken);
    }

    private async Task<ICollection<CourseTeacher>> ProcessTeacherString(ICollection<Teacher> teacher, ImportSignature s)
    {
        // Hint: 老師的英文名字可能會出錯
        // 如：Mei H.C. Ho,Vincent Kuo,Hsiao-Hui Chen,Sun-Jen Huang,Hwa-Meei Liou, John S. Liu => 六位老師
        // 如2: Yang, chuan-kai,Miao, Wei-Chung => 兩位老師
        var regex = new Regex(",(?!\\s)");
        string[] names = regex.Split(teacher.First().Name);
        string[] englishNames = regex.Split(teacher.First().EnglishName!);
        if (names.Length > englishNames.Length)
        {
            englishNames = teacher.First().EnglishName!.Split(',');
        }

        var correctTeachers = names
            .Select(n => new Teacher()
            {
                Name = n.Trim(),
                EnglishName = englishNames[names.ToList().IndexOf(n)].Trim(),
                EnglishNameInNtustCourse = englishNames[names.ToList().IndexOf(n)].Trim(),
                ImportSignatureId = s.Id
            }).ToList();

        for (int i = 0; i < correctTeachers.Count; i++)
        {
            var t =
                await _teacherRepo.FirstOrDefaultAsync(new GetTeacherByNameSpec(correctTeachers[i].Name, correctTeachers[i].EnglishNameInNtustCourse, s.Id));
            correctTeachers[i] = t ?? await _teacherRepo.AddAsync(correctTeachers[i]);
        }

        return correctTeachers.ConvertAll(t => new CourseTeacher()
        {
            TeacherId = t.Id
        });
    }
}
