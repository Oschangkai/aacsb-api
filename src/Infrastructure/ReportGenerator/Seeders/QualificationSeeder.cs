using System.Reflection;
using AACSB.WebApi.Application.Common.Interfaces;
using AACSB.WebApi.Domain.ReportGenerator;
using AACSB.WebApi.Infrastructure.Persistence.Context;
using AACSB.WebApi.Infrastructure.Persistence.Initialization;
using Microsoft.Extensions.Logging;

namespace AACSB.WebApi.Infrastructure.ReportGenerator.Seeders;

public class TeacherQualificationSeeder : ICustomSeeder
{
    private readonly ISerializerService _serializerService;
    private readonly ApplicationDbContext _db;
    private readonly ILogger<TeacherQualificationSeeder> _logger;

    public TeacherQualificationSeeder(ISerializerService serializerService, ILogger<TeacherQualificationSeeder> logger, ApplicationDbContext db)
    {
        _serializerService = serializerService;
        _logger = logger;
        _db = db;
    }

    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        string? path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        if (_db.Qualifications.Any()) return;

        _logger.LogInformation("Started to Seed Qualifications.");

        string qualificationData = await File.ReadAllTextAsync(path + "/ReportGenerator/Seeders/qualification.json", cancellationToken);

        var qualifications = _serializerService.Deserialize<List<Qualification>>(qualificationData);

        if (qualifications is { Count: > 0 })
        {
            foreach (var qualification in qualifications)
            {
                await _db.Qualifications.AddAsync(qualification, cancellationToken);
            }
        }

        await _db.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Seeded Qualifications.");
    }
}