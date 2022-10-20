using System.Reflection;
using AACSB.WebApi.Application.Common.Interfaces;
using AACSB.WebApi.Domain.ReportGenerator;
using AACSB.WebApi.Infrastructure.Persistence.Context;
using AACSB.WebApi.Infrastructure.Persistence.Initialization;
using Microsoft.Extensions.Logging;

namespace AACSB.WebApi.Infrastructure.ReportGenerator;

public class DisciplineSeeder : ICustomSeeder
{
    private readonly ISerializerService _serializerService;
    private readonly ApplicationDbContext _db;
    private readonly ILogger<DisciplineSeeder> _logger;

    public DisciplineSeeder(ISerializerService serializerService, ILogger<DisciplineSeeder> logger, ApplicationDbContext db)
    {
        _serializerService = serializerService;
        _logger = logger;
        _db = db;
    }

    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        string? path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        if (_db.Disciplines.Any()) return;

        _logger.LogInformation("Started to Seed Discipline.");

        string disciplineData = await File.ReadAllTextAsync(path + "/ReportGenerator/discipline.json", cancellationToken);

        var disciplines = _serializerService.Deserialize<List<Discipline>>(disciplineData);

        if (disciplines is { Count: > 0 })
        {
            foreach (var discipline in disciplines)
            {
                await _db.Disciplines.AddAsync(discipline, cancellationToken);
            }
        }

        await _db.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Seeded Discipline.");
    }
}