using AACSB.WebApi.Application.Common.Interfaces;
using AACSB.WebApi.Application.Common.Persistence;
using AACSB.WebApi.Application.ReportGenerator.Teachers;
using AACSB.WebApi.Shared.Notifications;
using Hangfire;
using Hangfire.Console.Extensions;
using Hangfire.Console.Progress;
using Hangfire.Server;
using Microsoft.Extensions.Logging;

namespace AACSB.WebApi.Infrastructure.ReportGenerator;

public class ImportTeacherInfoJob : IImportTeacherInfoJob
{
    private readonly ILogger<ImportTeacherInfoJob> _logger;
    private readonly IDapperRepository _repository;

    private readonly IProgressBarFactory _progressBar;
    private readonly PerformingContext _performingContext;
    private readonly INotificationSender _notifications;
    private readonly ICurrentUser _currentUser;
    private readonly IProgressBar _progress;

    public ImportTeacherInfoJob(
        ILogger<ImportTeacherInfoJob> logger,
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
    public async Task ImportAsync(List<TeacherInfo> teacherInfos, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Importing course disciplines...");
        await NotifyAsync("Importing researches...", 0, cancellationToken);
        _logger.LogInformation($"Course discipline mapping count: {teacherInfos.Count}");
        await NotifyAsync($"Course discipline mapping count: {teacherInfos.Count}", 10, cancellationToken);

        // Step 0: Start transaction & gathering ids
        using var trans = _repository.BeginTransaction();

        // Step 1: Create temp table
        _logger.LogInformation($"Creating temp table...");
        await NotifyAsync($"Creating temp table...", 15, cancellationToken);
        const string tempTableSql =
            "DROP TABLE IF EXISTS #TeacherInfo;\n" +
            "CREATE TABLE #TeacherInfo(\n" +
            "    Id uniqueidentifier,\n" +
            "    Name nvarchar(200),\n" +
            "    WorkTypeAbbr nvarchar(10),\n" +
            "    EnglishName nvarchar(200),\n" +
            "    Degree nvarchar(10),\n" +
            "    DegreeYear nvarchar(MAX),\n" +
            "    Department nvarchar(10),\n" +
            "    Email nvarchar(128),\n" +
            "    EnglishNameInNtustCourse nvarchar(200),\n" +
            "    NameInNtustCourse nvarchar(200),\n" +
            "    Qualification nvarchar(10),\n" +
            "    ResignDate date,\n" +
            "    Title nvarchar(80),\n" +
            "    Supervisor bit,\n" +
            "    LinkTo uniqueidentifier\n" +
            ");\n";
        int createTempTableResult = await _repository.ExecuteAsync(tempTableSql, transaction: trans, cancellationToken: cancellationToken);
        _logger.LogInformation($"=> {createTempTableResult}.");
        await NotifyAsync($"=> {createTempTableResult}.", 25, cancellationToken);

        // Step 2: Insert data to temp table
        _logger.LogInformation($"Insert {teacherInfos.Count} rows to temp table...");
        await NotifyAsync($"Insert {teacherInfos.Count} rows to temp table...", 30, cancellationToken);
        const string insertSql =
            "INSERT INTO #TeacherInfo (Id, Name, WorkTypeAbbr, EnglishName, Degree, DegreeYear, Department, Email, EnglishNameInNtustCourse, NameInNtustCourse, Qualification, ResignDate, Title, Supervisor, LinkTo)\n" +
            "VALUES (@Id, @Name, @WorkTypeAbbr, @EnglishName, @Degree, @DegreeYear, @Department, @Email, @EnglishNameInNtustCourse, @NameInNtustCourse, @Qualification, @ResignDate, @Title, @Supervisor, @LinkTo);\n";
        int insertResult = await _repository.ExecuteAsync(insertSql, teacherInfos, transaction: trans, cancellationToken: cancellationToken);
        _logger.LogInformation($"=> {insertResult} rows inserted to temp table.");
        await NotifyAsync($"=> {insertResult} rows inserted to temp table.", 50, cancellationToken);

        // Step 3: Update discipline
        _logger.LogInformation($"Update teacher info...");
        await NotifyAsync($"Update teacher info...", 55, cancellationToken);
        const string updateSql =
            "WITH CTE AS (\n" +
            "    SELECT ti.*,\n" +
            "           tti.NameInNtustCourse AS LinkToNameInNtustCourse,\n" +
            "           tti.EnglishNameInNtustCourse AS LinkToEnglishNameInNtustCourse\n" +
            "    FROM #TeacherInfo ti\n" +
            "             LEFT JOIN #TeacherInfo tti ON ti.LinkTo = tti.Id\n" +
            ")\n" +
            "UPDATE t\n" +
            "SET t.Name = ti.Name,\n" +
            "    t.EnglishName = ti.EnglishName,\n" +
            "    t.WorkTypeAbbr = ti.WorkTypeAbbr,\n" +
            "    t.Degree = ti.Degree,\n" +
            "    t.DegreeYear = ti.DegreeYear,\n" +
            "    t.DepartmentId = d.Id,\n" +
            "    t.Email = ti.Email,\n" +
            "    t.QualificationId = q.Id,\n" +
            "    t.ResignDate = ti.ResignDate,\n" +
            "    t.Title = ti.Title,\n" +
            "    t.Supervisor = ti.Supervisor,\n" +
            "    t.LinkTo = tti.Id\n" +
            "FROM ReportGenerator.Teachers t\n" +
            "  LEFT JOIN CTE ti ON t.NameInNtustCourse = ti.NameInNtustCourse AND t.EnglishNameInNtustCourse = ti.EnglishNameInNtustCourse\n" +
            "  LEFT JOIN CTE tti ON t.NameInNtustCourse = tti.LinkToNameInNtustCourse AND t.EnglishNameInNtustCourse = tti.LinkToEnglishNameInNtustCourse\n" +
            "  LEFT JOIN ReportGenerator.Departments d ON ti.Department = d.Abbreviation\n" +
            "  LEFT JOIN ReportGenerator.Qualifications q ON ti.Qualification = q.Abbreviation\n" +
            "  LEFT JOIN ReportGenerator.Teachers t2 ON ti.LinkTo = t2.Id\n" +
            "WHERE ti.Id IS NOT NULL";
        int updateResult = await _repository.ExecuteAsync(updateSql, transaction: trans, cancellationToken: cancellationToken);
        _logger.LogInformation($"=> {updateResult}.");
        await NotifyAsync($"=> {updateResult}.", 85, cancellationToken);

        // Step 4: Commit transaction
        trans.Commit();
        _logger.LogInformation($"transaction committed.");
        await NotifyAsync($"transaction committed.", 100, cancellationToken);
    }
}