using System.Text.Json;
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
using MediatR;
using Microsoft.Extensions.Logging;

namespace AACSB.WebApi.Infrastructure.ReportGenerator;

public class FetchCoursesJob : IFetchCourseJob
{
    private readonly ILogger<FetchCoursesJob> _logger;
    private readonly ISender _mediator;
    private readonly IRepositoryWithEvents<Course> _courseRepo;
    private readonly IRepositoryWithEvents<Teacher> _teacherRepo;
    private readonly IReadRepository<Department> _departmentRepo;
    private readonly IRepositoryWithEvents<ImportSignature> _importSignatureRepo;
    private readonly IProgressBarFactory _progressBar;
    private readonly PerformingContext _performingContext;
    private readonly INotificationSender _notifications;
    private readonly ICurrentUser _currentUser;
    private readonly IProgressBar _progress;
    public FetchCoursesJob(
        ILogger<FetchCoursesJob> logger,
        ISender mediator,
        IRepositoryWithEvents<Course> courseRepo,
        IRepositoryWithEvents<Teacher> teacherRepo,
        IReadRepository<Department> departmentRepo,
        IRepositoryWithEvents<ImportSignature> importSignatureRepo,
        IProgressBarFactory progressBar,
        PerformingContext performingContext,
        INotificationSender notifications,
        ICurrentUser currentUser)
    {
        _logger = logger;
        _mediator = mediator;
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
            var department = await _departmentRepo.GetBySpecAsync(new GetDepartmentByAbbrSpec(dept), cancellationToken);
            _logger.LogInformation($"Fetch courses: {year}{semester}, {dept} with signature {importSignature.Name}");

            // Task3.1: Fetch Chinese and English course
            var (chineseCourseString, englishCourseString) = await request.GetCourseByDepartment($"{year}{semester}", dept, cancellationToken);
            var chineseCourses = JsonSerializer.Deserialize<List<CourseResponse>>(await chineseCourseString);
            var englishCourses = JsonSerializer.Deserialize<List<CourseResponse>>(await englishCourseString);

            // Task3.2: Merge Chinese and English course
            var courses =
                from cc in chineseCourses
                join ec in englishCourses on cc.CourseNo equals ec.CourseNo
                select new Course()
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
                    Teachers = cc.CourseTeacher.Contains(',')
                        ? cc.CourseTeacher.Split(',')
                            .Select((t, i) => new Teacher()
                            {
                                Name = t,
                                EnglishName = ec.CourseTeacher.Split(',')[i]
                            }).ToList()
                        : new List<Teacher>() { new Teacher() { Name = cc.CourseTeacher, EnglishName = ec.CourseTeacher} }
                };

            var cl = courses.ToList();
            _logger.LogInformation($"{cl.Count()} courses will be inserted.");
            courseList.AddRange(cl);
        }


        // Task4: Insert courses
        _logger.LogInformation($"({year}{semester}) semester has {courseList.Count} courses will be inserted with signature {importSignature.Name}");
        await _courseRepo.AddRangeAsync(courseList, cancellationToken);
    }
}

