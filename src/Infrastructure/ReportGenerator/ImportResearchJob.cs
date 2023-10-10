using System.Text.Json;
using AACSB.WebApi.Application.Common.Importer;
using AACSB.WebApi.Application.Common.Interfaces;
using AACSB.WebApi.Application.Common.Persistence;
using AACSB.WebApi.Application.ReportGenerator.Research;
using AACSB.WebApi.Domain.ReportGenerator;
using AACSB.WebApi.Shared.Notifications;
using Hangfire;
using Hangfire.Console.Extensions;
using Hangfire.Console.Progress;
using Hangfire.Server;
using Microsoft.Extensions.Logging;

namespace AACSB.WebApi.Infrastructure.ReportGenerator;

public class ImportResearchJob : IImportResearchJob
{
    private readonly ILogger<ImportResearchJob> _logger;
    private readonly IRepository<Research> _researchRepo;
    private readonly IReadRepository<Teacher> _teacherRepo;
    private readonly IReadRepository<ResearchType> _researchTypeRepo;
    private readonly IExcelReader _excelReader;

    private readonly IProgressBarFactory _progressBar;
    private readonly PerformingContext _performingContext;
    private readonly INotificationSender _notifications;
    private readonly ICurrentUser _currentUser;
    private readonly IProgressBar _progress;

    public ImportResearchJob(
        ILogger<ImportResearchJob> logger,
        IRepository<Research> researchRepo,
        IReadRepository<Teacher> teacherRepo,
        IReadRepository<ResearchType> researchTypeRepo,
        IExcelReader excelReader,
        IProgressBarFactory progressBar,
        PerformingContext performingContext,
        INotificationSender notifications,
        ICurrentUser currentUser)
    {
        _logger = logger;
        _researchRepo = researchRepo;
        _teacherRepo = teacherRepo;
        _researchTypeRepo = researchTypeRepo;
        _excelReader = excelReader;
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
    public async Task ImportAsync(List<string> files, CancellationToken cancellationToken)
    {
        // Step 0: notify start
        _logger.LogInformation("Importing researches...");
        await NotifyAsync("Importing researches...", 0, cancellationToken);
        _logger.LogInformation($"Files count: {files.Count}");
        await NotifyAsync($"Files count: {files.Count}", 10, cancellationToken);

        // Step 1: read files from xlsx and convert to Research object
        List<Research> importedResearches = new();
        for (int i = 0; i < files.Count; i++)
        {
            _logger.LogInformation($"Importing file {i + 1} of {files.Count}\npath: {files[i]}");
            await NotifyAsync($"Importing file {i + 1} of {files.Count}: {files[i]}", 10 + ((i + 1) * 60 / files.Count) - 1, cancellationToken);
            var r = _excelReader.ReadFromFilePath<Research>(files[i]);
            importedResearches.AddRange(r);

            _logger.LogInformation($"Researches count: {r.Count}");
            await NotifyAsync($"Researches count: {r.Count}", 10 + ((i + 1) * 60 / files.Count), cancellationToken);
        }

        _logger.LogInformation($"{importedResearches.Count} researches pending imported");
        await NotifyAsync($"{importedResearches.Count} researches pending imported", 70, cancellationToken);

        // Step 2.1: Get teacher list and research types list from database
        List<Research> researches = new();
        var teachers = await _teacherRepo.ListAsync(cancellationToken);
        var researchTypes = await _researchTypeRepo.ListAsync(cancellationToken);

        // Step 2.2: normalize researches
        for (int index = 0; index < importedResearches.Count; index++)
        {
            var ir = importedResearches[index];
            var r = new Research()
            {
                Type = ir.Type,
                Title = ir.Title,
                OtherAuthors = ir.OtherAuthors,
                OrderAuthors = ir.OtherAuthors,
                AddressAuthors = ir.AddressAuthors,
                Publication = ir.Publication,
                Seminar = ir.Seminar,
                YearStart = ir.YearStart,
                MonthStart = ir.MonthStart,
                DayStart = ir.DayStart,
                YearEnd = ir.YearEnd,
                MonthEnd = ir.MonthEnd,
                DayEnd = ir.DayEnd,
                Year = ir.Year,
                Month = ir.Month,
                Country = ir.Country,
                City = ir.City,
                PageStart = ir.PageStart,
                PageEnd = ir.PageEnd,
                Project = ir.Project,
                FullText = ir.FullText,
                JournalsName = ir.JournalsName,
                JournalsType = ir.JournalsType,
                JournalsClass = ir.JournalsClass,
                JournalsStatus = ir.JournalsStatus,
                Volume = ir.Volume,
                Issue = ir.Issue,
                Portfolio = ir.Portfolio // deprecated
            };

            // Step 2.2.1: Teacher
            var teacher = teachers.Find(x => x.Name == ir.Teacher.Name);
            if (teacher is not null)
            {
                r.TeacherId = teacher.Id;
            }
            else
            {
                _logger.LogInformation($"Teacher '{ir.Teacher.Name}' not found, record will be ignored '{ir.Title}'");
                continue;
            }

            // Step 2.2.2: ResearchTypes
            if (ir.ResearchTypes is not null)
            {
                var researchTypeCodeList = ir.ResearchTypes.Select(x => x.ResearchType.Code).ToList();
                // TODO: pending deprecated research type column removed
                switch (ir.Type)
                {
                    case "Journal 1":
                    case "Journal 2":
                        researchTypeCodeList.Add("A");
                        break;
                    case "Presentation":
                        researchTypeCodeList.Add("B");
                        break;
                    case "Proceeding":
                        researchTypeCodeList.Add("C");
                        break;
                }

                var researchTypeList = researchTypes.Where(x => researchTypeCodeList.Contains(x.Code)).ToList();
                r.ResearchTypes = researchTypeList.ConvertAll(x => new ResearchResearchType() { ResearchTypeId = x.Id });
            }

            researches.Add(r);
            await NotifyAsync($"research pass", 70 + ((index + 1) * 20 / importedResearches.Count), cancellationToken);
        }

        // Step 3: save to database
        _logger.LogInformation($"{researches.Count} researches pending imported");
        await NotifyAsync($"{researches.Count} researches pending imported", 95, cancellationToken);
        var added = await _researchRepo.AddRangeAsync(researches, cancellationToken);

        _logger.LogInformation($"{added.Count()} researches imported");
        await NotifyAsync($"{added.Count()} researches imported", 100, cancellationToken);
    }
}