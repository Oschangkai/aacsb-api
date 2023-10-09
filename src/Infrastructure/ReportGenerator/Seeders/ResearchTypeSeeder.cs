using System.Reflection;
using AACSB.WebApi.Application.Common.Interfaces;
using AACSB.WebApi.Domain.ReportGenerator;
using AACSB.WebApi.Infrastructure.Persistence.Context;
using AACSB.WebApi.Infrastructure.Persistence.Initialization;
using Microsoft.Extensions.Logging;

namespace AACSB.WebApi.Infrastructure.ReportGenerator.Seeders;

public class ResearchTypeSeeder : ICustomSeeder
{
    private readonly ISerializerService _serializerService;
    private readonly ApplicationDbContext _db;
    private readonly ILogger<ResearchTypeSeeder> _logger;

    public ResearchTypeSeeder(ISerializerService serializerService, ILogger<ResearchTypeSeeder> logger, ApplicationDbContext db)
    {
        _serializerService = serializerService;
        _logger = logger;
        _db = db;
    }

    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        string? path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        if (_db.ResearchTypes.Any()) return;

        _logger.LogInformation("Started to Seed ResearchType.");

        string ResearchTypeData = await File.ReadAllTextAsync(path + "/ReportGenerator/Seeders/researchType.json", cancellationToken);

        var ResearchTypes = _serializerService.Deserialize<List<ResearchType>>(ResearchTypeData);

        if (ResearchTypes is { Count: > 0 })
        {
            foreach (var researchType in ResearchTypes)
            {
                await _db.ResearchTypes.AddAsync(researchType, cancellationToken);
            }
        }

        await _db.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Seeded ResearchType.");
    }
}