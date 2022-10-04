using Finbuckle.MultiTenant;
using AACSB.WebApi.Application.Common.Events;
using AACSB.WebApi.Application.Common.Interfaces;
using AACSB.WebApi.Domain.Catalog;
using AACSB.WebApi.Infrastructure.Persistence.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AACSB.WebApi.Infrastructure.Persistence.Context;

public class ApplicationDbContext : BaseDbContext
{
    public ApplicationDbContext(ITenantInfo currentTenant, DbContextOptions options, ICurrentUser currentUser, ISerializerService serializer, IOptions<DatabaseSettings> dbSettings, IEventPublisher events)
        : base(currentTenant, options, currentUser, serializer, dbSettings, events)
    {
    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Brand> Brands => Set<Brand>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Teacher> Teachers => Set<Teacher>();
    public DbSet<Discipline> Disciplines => Set<Discipline>();
    public DbSet<ImportSignature> ImportSignatures => Set<ImportSignature>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema(SchemaNames.Catalog);
    }
}