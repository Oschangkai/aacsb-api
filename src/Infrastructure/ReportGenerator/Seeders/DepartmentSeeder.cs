using System.Reflection;
using AACSB.WebApi.Application.Common.Interfaces;
using AACSB.WebApi.Domain.ReportGenerator;
using AACSB.WebApi.Infrastructure.Persistence.Context;
using AACSB.WebApi.Infrastructure.Persistence.Initialization;
using Microsoft.Extensions.Logging;

namespace AACSB.WebApi.Infrastructure.ReportGenerator.Seeders;

public class DepartmentSeeder : ICustomSeeder
{
    private readonly ISerializerService _serializerService;
    private readonly ApplicationDbContext _db;
    private readonly ILogger<DepartmentSeeder> _logger;

    public DepartmentSeeder(ISerializerService serializerService, ILogger<DepartmentSeeder> logger, ApplicationDbContext db)
    {
        _serializerService = serializerService;
        _logger = logger;
        _db = db;
    }

    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        string? path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        if (_db.Departments.Any()) return;

        _logger.LogInformation("Started to Seed Department.");

        string departmentData = await File.ReadAllTextAsync(path + "/ReportGenerator/Seeders/department.json", cancellationToken);

        var departments = _serializerService.Deserialize<List<Department>>(departmentData);

        if (departments is { Count: > 0 })
        {
            foreach (var department in departments)
            {
                await _db.Departments.AddAsync(department, cancellationToken);
            }
        }

        await _db.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Seeded Department.");
    }
}