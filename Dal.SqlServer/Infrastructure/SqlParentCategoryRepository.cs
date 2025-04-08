using Dapper;
using Domain.Entities;
using Repository.Repositories;
using System.Data;
using System.Data.Common;

namespace Dal.SqlServer.Infrastructure;

public class SqlParentCategoryRepository : IParentCategoryRepository
{
    private readonly IDbConnection _db;

    public SqlParentCategoryRepository(IDbConnection db)
    {
        _db = db;
    }

    public async Task<IEnumerable<ParentCategory>> GetAllAsync()
    {
        const string sql = "SELECT * FROM ParentCategory";
        return await _db.QueryAsync<ParentCategory>(sql);
    }

    public async Task<ParentCategory?> GetByIdAsync(int id)
    {
        const string sql = "SELECT * FROM ParentCategory WHERE Id = @Id";
        return await _db.QueryFirstOrDefaultAsync<ParentCategory>(sql, new { Id = id });
    }

    public async Task<ParentCategory?> GetByNameAsync(string name)
    {
        const string sql = "SELECT * FROM ParentCategory WHERE Name = @name";
        return await _db.QueryFirstOrDefaultAsync<ParentCategory>(sql, new { name });
    }


}
