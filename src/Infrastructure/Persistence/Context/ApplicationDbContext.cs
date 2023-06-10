using System.Reflection;
using Finbuckle.MultiTenant;
using AACSB.WebApi.Application.Common.Events;
using AACSB.WebApi.Application.Common.Interfaces;
using AACSB.WebApi.Domain.Catalog;
using AACSB.WebApi.Domain.ReportGenerator.Function;
using AACSB.WebApi.Domain.ReportGenerator.View;
using AACSB.WebApi.Infrastructure.Persistence.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AACSB.WebApi.Infrastructure.Persistence.Context;

public partial class ApplicationDbContext : BaseDbContext
{
    public ApplicationDbContext(ITenantInfo currentTenant, DbContextOptions options, ICurrentUser currentUser, ISerializerService serializer, IOptions<DatabaseSettings> dbSettings, IEventPublisher events)
        : base(currentTenant, options, currentUser, serializer, dbSettings, events)
    {
    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Brand> Brands => Set<Brand>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema(SchemaNames.Catalog);

        modelBuilder
            .Entity<TableA31Course>()
            .ToView("V_Table_A31_Course", SchemaNames.ReportGenerator)
            .HasNoKey();

        modelBuilder.Entity<TableA31>().HasNoKey();
        modelBuilder
            .HasDbFunction(
                typeof(ApplicationDbContext).GetMethod(nameof(GetTeacherDiscipline), new[] { typeof(string) })!)
            .HasName("F_GetTeacherDiscipline")
            .HasParameter("SemesterYear").HasStoreType("nvarchar(3)");

        modelBuilder.Entity<TableA32>().HasNoKey();
        modelBuilder
            .HasDbFunction(
                typeof(ApplicationDbContext).GetMethod(nameof(GetQualificationPercentage), new[] { typeof(string), typeof(string) })!,
                builder =>
                {
                    builder.HasName("F_GetQualificationPercentage");
                    builder.HasSchema(nameof(TableA32));
                    builder.HasParameter("Semester").HasStoreType("varchar(4)");
                    builder.HasParameter("Type").HasStoreType("varchar(10)");
                });
    }
}