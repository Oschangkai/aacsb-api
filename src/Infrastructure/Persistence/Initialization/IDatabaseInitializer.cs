using AACSB.WebApi.Infrastructure.Multitenancy;

namespace AACSB.WebApi.Infrastructure.Persistence.Initialization;

internal interface IDatabaseInitializer
{
    Task InitializeDatabasesAsync(CancellationToken cancellationToken);
    Task InitializeApplicationDbForTenantAsync(AACSBTenantInfo tenant, CancellationToken cancellationToken);
}