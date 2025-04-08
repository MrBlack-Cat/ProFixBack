using Dapper;
using Domain.Entities;
using Repository.Repositories;
using System.Data;

namespace Dal.SqlServer.Infrastructure;

public class SqlServiceTypeRepository : IServiceTypeRepository
{
    private readonly IDbConnection _db;

    public SqlServiceTypeRepository(IDbConnection db)
    {
        _db = db;
    }

    public async Task<IEnumerable<ServiceType>> GetAllAsync()
    {
        const string sql = "SELECT * FROM ServiceType";
        return await _db.QueryAsync<ServiceType>(sql);
    }

    public async Task<ServiceType?> GetByIdAsync(int id)
    {
        const string sql = "SELECT * FROM ServiceType WHERE Id = @Id";
        return await _db.QueryFirstOrDefaultAsync<ServiceType>(sql, new { Id = id });
    }

    public async Task<IEnumerable<ServiceType>> GetByParentCategoryIdAsync(int parentCategoryId)
    {
        const string sql = "SELECT * FROM ServiceType WHERE ParentCategoryId = @ParentCategoryId";
        return await _db.QueryAsync<ServiceType>(sql, new { ParentCategoryId = parentCategoryId });
    }

    public async Task<List<ServiceType>> SearchByNameAsync(string name)
    {
        const string sql = "SELECT * FROM ServiceType WHERE Name LIKE @Name AND ParentCategoryId IS NOT NULL";
        return (await _db.QueryAsync<ServiceType>(sql, new { Name = $"%{name}%" })).ToList();
    }
}
