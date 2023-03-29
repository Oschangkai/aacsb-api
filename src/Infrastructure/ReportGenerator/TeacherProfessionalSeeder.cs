using System.Reflection;
using AACSB.WebApi.Application.Common.Interfaces;
using AACSB.WebApi.Domain.ReportGenerator;
using AACSB.WebApi.Infrastructure.Persistence.Context;
using AACSB.WebApi.Infrastructure.Persistence.Initialization;
using Microsoft.Extensions.Logging;

namespace AACSB.WebApi.Infrastructure.ReportGenerator;

public class TeacherProfessionalSeeder : ICustomSeeder
{
    private readonly ISerializerService _serializerService;
    private readonly ApplicationDbContext _db;
    private readonly ILogger<TeacherProfessionalSeeder> _logger;

    public TeacherProfessionalSeeder(ISerializerService serializerService, ILogger<TeacherProfessionalSeeder> logger, ApplicationDbContext db)
    {
        _serializerService = serializerService;
        _logger = logger;
        _db = db;
    }

    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        string? path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        if (_db.TeacherProfessionals.Any()) return;

        _logger.LogInformation("Started to Seed Teacher Professional.");

        string professionalData = await File.ReadAllTextAsync(path + "/ReportGenerator/professional.json", cancellationToken);

        var professionals = _serializerService.Deserialize<List<TeacherProfessional>>(professionalData);

        if (professionals is { Count: > 0 })
        {
            foreach (var professional in professionals)
            {
                await _db.TeacherProfessionals.AddAsync(professional, cancellationToken);
            }
        }

        await _db.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Seeded Teacher Professionals.");
    }
}