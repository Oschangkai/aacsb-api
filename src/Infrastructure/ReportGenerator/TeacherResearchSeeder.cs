using System.Reflection;
using AACSB.WebApi.Application.Common.Interfaces;
using AACSB.WebApi.Domain.ReportGenerator;
using AACSB.WebApi.Infrastructure.Persistence.Context;
using AACSB.WebApi.Infrastructure.Persistence.Initialization;
using Microsoft.Extensions.Logging;

namespace AACSB.WebApi.Infrastructure.ReportGenerator;

public class TeacherResearchSeeder : ICustomSeeder
{
    private readonly ISerializerService _serializerService;
    private readonly ApplicationDbContext _db;
    private readonly ILogger<TeacherResearchSeeder> _logger;

    public TeacherResearchSeeder(ISerializerService serializerService, ILogger<TeacherResearchSeeder> logger, ApplicationDbContext db)
    {
        _serializerService = serializerService;
        _logger = logger;
        _db = db;
    }

    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        string? path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        if (_db.TeacherResearch.Any()) return;

        _logger.LogInformation("Started to Seed Teacher Research.");

        string researchData = await File.ReadAllTextAsync(path + "/ReportGenerator/research.json", cancellationToken);

        var research = _serializerService.Deserialize<List<TeacherResearch>>(researchData);

        if (research is { Count: > 0 })
        {
            foreach (var r in research)
            {
                await _db.TeacherResearch.AddAsync(r, cancellationToken);
            }
        }

        await _db.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Seeded Teacher Research.");
    }
}