using Dapper;
using Domain.Entities;
using Domain.Types;
using Repository.Repositories;
using System.Data;
using System.Data.Common;

namespace Dal.SqlServer.Infrastructure;

public class SqlServiceProviderServiceTypeRepository : IServiceProviderServiceTypeRepository
{
    private readonly IDbConnection _db;

    public SqlServiceProviderServiceTypeRepository(IDbConnection db)
    {
        _db = db;
    }

    public async Task<List<ServiceProviderServiceType>> GetByServiceProviderProfileIdAsync(int profileId)
    {
        const string sql = "SELECT * FROM ServiceProviderServiceTypes WHERE ServiceProviderProfileId = @ProfileId";
        var result = await _db.QueryAsync<ServiceProviderServiceType>(sql, new { ProfileId = profileId });
        return result.ToList();
    }

    public async Task AddAsync(ServiceProviderServiceType entity)
    {
        const string sql = @"
            INSERT INTO ServiceProviderServiceTypes (ServiceProviderProfileId, ServiceTypeId)
            VALUES (@ServiceProviderProfileId, @ServiceTypeId)";
        await _db.ExecuteAsync(sql, entity);
    }

    public async Task AddRangeAsync(List<ServiceProviderServiceType> entities)
    {
        const string sql = @"
            INSERT INTO ServiceProviderServiceTypes (ServiceProviderProfileId, ServiceTypeId)
            VALUES (@ServiceProviderProfileId, @ServiceTypeId)";
        await _db.ExecuteAsync(sql, entities);
    }

    public async Task DeleteAllByServiceProviderProfileIdAsync(int profileId)
    {
        Console.WriteLine(": " + profileId);
        var sql = "DELETE FROM ServiceProviderServiceTypes WHERE ServiceProviderProfileId = @ServiceProviderProfileId";
        await _db.ExecuteAsync(sql, new { ServiceProviderProfileId = profileId });
    }

}
