using System.Data;
using Dapper;
using Finbuckle.MultiTenant.EntityFrameworkCore;
using AACSB.WebApi.Application.Common.Persistence;
using AACSB.WebApi.Domain.Common.Contracts;
using AACSB.WebApi.Infrastructure.Persistence.Context;

namespace AACSB.WebApi.Infrastructure.Persistence.Repository;

public class DapperRepository : IDapperRepository
{
    private readonly ApplicationDbContext _dbContext;

    public DapperRepository(ApplicationDbContext dbContext) => _dbContext = dbContext;

    public async Task<int> ExecuteAsync(string sql, object? param = null, IDbTransaction? transaction = null, CancellationToken cancellationToken = default)
        => await _dbContext.Connection.ExecuteAsync(sql, param, transaction);

    public IDbTransaction BeginTransaction()
    {
        var connection = _dbContext.Connection;
        connection.Open();
        return connection.BeginTransaction();
    }

    public async Task<int> UpdateAsync(string sql, object? param = null, IDbTransaction? transaction = null, CancellationToken cancellationToken = default)
        => await _dbContext.Connection.ExecuteAsync(sql, param, transaction);

    public async Task<IReadOnlyList<T>> QueryAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, CancellationToken cancellationToken = default)
     =>
        (await _dbContext.Connection.QueryAsync<T>(sql, param, transaction))
            .AsList();
    public async Task QueryAsync(string sql, object? param = null, IDbTransaction? transaction = null, CancellationToken cancellationToken = default)
        => await _dbContext.Connection.QueryAsync(sql, param, transaction);

    public async Task<T?> QueryFirstOrDefaultAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, CancellationToken cancellationToken = default)
    where T : class
    {
        if (_dbContext.Model.GetMultiTenantEntityTypes().Any(t => t.ClrType == typeof(T)))
        {
            sql = sql.Replace("@tenant", _dbContext.TenantInfo.Id);
        }

        return await _dbContext.Connection.QueryFirstOrDefaultAsync<T>(sql, param, transaction);
    }

    public Task<T> QuerySingleAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, CancellationToken cancellationToken = default)
    where T : class
    {
        if (_dbContext.Model.GetMultiTenantEntityTypes().Any(t => t.ClrType == typeof(T)))
        {
            sql = sql.Replace("@tenant", _dbContext.TenantInfo.Id);
        }

        return _dbContext.Connection.QuerySingleAsync<T>(sql, param, transaction);
    }
}