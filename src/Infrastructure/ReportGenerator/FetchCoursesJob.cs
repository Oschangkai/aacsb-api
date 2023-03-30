using AACSB.WebApi.Application.Common.Interfaces;
using AACSB.WebApi.Application.Common.Persistence;
using AACSB.WebApi.Application.ReportGenerator;
using AACSB.WebApi.Domain.ReportGenerator;
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
    private readonly IReadRepository<Course> _courseRepo;
    private readonly IReadRepository<Teacher> _teacherRepo;
    private readonly IProgressBarFactory _progressBar;
    private readonly PerformingContext _performingContext;
    private readonly INotificationSender _notifications;
    private readonly ICurrentUser _currentUser;
    private readonly IProgressBar _progress;
    public FetchCoursesJob(
        ILogger<FetchCoursesJob> logger,
        ISender mediator,
        IReadRepository<Course> courseRepo,
        IReadRepository<Teacher> teacherRepo,
        IProgressBarFactory progressBar,
        PerformingContext performingContext,
        INotificationSender notifications,
        ICurrentUser currentUser)
    {
        _logger = logger;
        _mediator = mediator;
        _courseRepo = courseRepo;
        _teacherRepo = teacherRepo;
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
    public async Task FetchAsync(int year, int semester, string[] department, CancellationToken cancellationToken)
    {
        if (department.Length <= 0) return;

        // Task1: Fetch courses


        await NotifyAsync($"Fetch courses started: {year}, {semester}, {department}", 0, cancellationToken);
    }
}