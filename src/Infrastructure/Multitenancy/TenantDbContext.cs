using Finbuckle.MultiTenant.Stores;
using AACSB.WebApi.Infrastructure.Persistence.Configuration;
using Microsoft.EntityFrameworkCore;

namespace AACSB.WebApi.Infrastructure.Multitenancy;

public class TenantDbContext : EFCoreStoreDbContext<AACSBTenantInfo>
{
    public TenantDbContext(DbContextOptions<TenantDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AACSBTenantInfo>().ToTable("Tenants", SchemaNames.MultiTenancy);
    }
}