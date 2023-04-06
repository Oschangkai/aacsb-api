using System.Reflection;
using AACSB.WebApi.Application.Common.Interfaces;
using AACSB.WebApi.Domain.ReportGenerator;
using AACSB.WebApi.Infrastructure.Persistence.Context;
using AACSB.WebApi.Infrastructure.Persistence.Initialization;
using Microsoft.Extensions.Logging;

namespace AACSB.WebApi.Infrastructure.ReportGenerator.Seeders;

public class ProfessionalSeeder : ICustomSeeder
{
    private readonly ISerializerService _serializerService;
    private readonly ApplicationDbContext _db;
    private readonly ILogger<ProfessionalSeeder> _logger;

    public ProfessionalSeeder(ISerializerService serializerService, ILogger<ProfessionalSeeder> logger, ApplicationDbContext db)
    {
        _serializerService = serializerService;
        _logger = logger;
        _db = db;
    }

    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        string? path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        if (_db.Professionals.Any()) return;

        _logger.LogInformation("Started to Seed Professionals.");

        string professionalData = await File.ReadAllTextAsync(path + "/ReportGenerator/Seeders/professional.json", cancellationToken);

        var professionals = _serializerService.Deserialize<List<Professional>>(professionalData);

        if (professionals is { Count: > 0 })
        {
            foreach (var professional in professionals)
            {
                await _db.Professionals.AddAsync(professional, cancellationToken);
            }
        }

        await _db.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Seeded Professionals.");
    }
}