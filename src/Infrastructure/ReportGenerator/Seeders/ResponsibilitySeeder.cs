using System.Reflection;
using AACSB.WebApi.Application.Common.Interfaces;
using AACSB.WebApi.Domain.ReportGenerator;
using AACSB.WebApi.Infrastructure.Persistence.Context;
using AACSB.WebApi.Infrastructure.Persistence.Initialization;
using Microsoft.Extensions.Logging;

namespace AACSB.WebApi.Infrastructure.ReportGenerator.Seeders;

public class TeacherResponsibilitySeeder : ICustomSeeder
{
    private readonly ISerializerService _serializerService;
    private readonly ApplicationDbContext _db;
    private readonly ILogger<TeacherResponsibilitySeeder> _logger;

    public TeacherResponsibilitySeeder(ISerializerService serializerService, ILogger<TeacherResponsibilitySeeder> logger, ApplicationDbContext db)
    {
        _serializerService = serializerService;
        _logger = logger;
        _db = db;
    }

    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        string? path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        if (_db.Responsibilities.Any()) return;

        _logger.LogInformation("Started to Seed Responsibilities.");

        string responsibilityData = await File.ReadAllTextAsync(path + "/ReportGenerator/Seeders/responsibility.json", cancellationToken);

        var responsibilities = _serializerService.Deserialize<List<Responsibility>>(responsibilityData);

        if (responsibilities is { Count: > 0 })
        {
            foreach (var responsibility in responsibilities)
            {
                await _db.Responsibilities.AddAsync(responsibility, cancellationToken);
            }
        }

        await _db.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Seeded Responsibilities.");
    }
}