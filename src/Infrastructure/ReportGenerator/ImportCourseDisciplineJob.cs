using AACSB.WebApi.Application.Common.Interfaces;
using AACSB.WebApi.Application.Common.Persistence;
using AACSB.WebApi.Application.ReportGenerator.Courses;
using AACSB.WebApi.Shared.Notifications;
using Hangfire;
using Hangfire.Console.Extensions;
using Hangfire.Console.Progress;
using Hangfire.Server;
using Microsoft.Extensions.Logging;

namespace AACSB.WebApi.Infrastructure.ReportGenerator;

public class ImportCourseDisciplineJob : IImportCourseDisciplineJob
{
    private readonly ILogger<ImportCourseDisciplineJob> _logger;
    private readonly IDapperRepository _repository;

    private readonly IProgressBarFactory _progressBar;
    private readonly PerformingContext _performingContext;
    private readonly INotificationSender _notifications;
    private readonly ICurrentUser _currentUser;
    private readonly IProgressBar _progress;

    public ImportCourseDisciplineJob(
        ILogger<ImportCourseDisciplineJob> logger,
        IDapperRepository repository,
        IProgressBarFactory progressBar,
        PerformingContext performingContext,
        INotificationSender notifications,
        ICurrentUser currentUser)
    {
        _logger = logger;
        _repository = repository;
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

    [Queue("importer")]
    [AutomaticRetry(Attempts = 0)]
    public async Task ImportAsync(List<CourseDiscipline> courseDisciplines, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Importing course disciplines...");
        await NotifyAsync("Importing researches...", 0, cancellationToken);
        _logger.LogInformation($"Course discipline mapping count: {courseDisciplines.Count}");
        await NotifyAsync($"Course discipline mapping count: {courseDisciplines.Count}", 10, cancellationToken);

        // Step 0: Start transaction
        using var trans = _repository.BeginTransaction();

        // Step 1: Create temp table
        _logger.LogInformation($"Creating temp table...");
        await NotifyAsync($"Creating temp table...", 15, cancellationToken);
        const string tempTableSql =
            "DROP TABLE IF EXISTS #CourseDiscipline;\n" +
            "CREATE TABLE #CourseDiscipline(\n" +
            "    Code nvarchar(256),\n" +
            "    DisciplineCode int,\n" +
            "    DisciplineName nvarchar(MAX)\n" +
            ");\n";
        int createTempTableResult = await _repository.ExecuteAsync(tempTableSql, transaction: trans, cancellationToken: cancellationToken);
        _logger.LogInformation($"=> {createTempTableResult}.");
        await NotifyAsync($"=> {createTempTableResult}.", 25, cancellationToken);

        // Step 2: Insert data to temp table
        _logger.LogInformation($"Insert {courseDisciplines.Count} rows to temp table...");
        await NotifyAsync($"Insert {courseDisciplines.Count} rows to temp table...", 30, cancellationToken);
        const string insertSql =
            "INSERT INTO #CourseDiscipline (Code, DisciplineCode, DisciplineName)\n" +
            "VALUES (@Code, @DisciplineCode, @DisciplineName);\n";
        int insertResult = await _repository.ExecuteAsync(insertSql, courseDisciplines, transaction: trans, cancellationToken: cancellationToken);
        _logger.LogInformation($"=> {insertResult} rows inserted to temp table.");
        await NotifyAsync($"=> {insertResult} rows inserted to temp table.", 50, cancellationToken);

        // Step 3: Update discipline
        _logger.LogInformation($"Update discipline...");
        await NotifyAsync($"Update discipline...", 55, cancellationToken);
        const string updateSql =
            "UPDATE c\n" +
            "SET c.DisciplineId = d.Id\n" +
            "FROM ReportGenerator.Courses c\n" +
            "    LEFT JOIN #CourseDiscipline cd on c.Code = cd.Code\n" +
            "    LEFT JOIN ReportGenerator.Discipline d on cd.DisciplineCode = d.Code;";
        int updateResult = await _repository.ExecuteAsync(updateSql, transaction: trans, cancellationToken: cancellationToken);
        _logger.LogInformation($"=> {updateResult}.");
        await NotifyAsync($"=> {updateResult}.", 85, cancellationToken);

        // Step 4: Commit transaction
        trans.Commit();
        _logger.LogInformation($"transaction committed.");
        await NotifyAsync($"transaction committed.", 100, cancellationToken);
    }
}