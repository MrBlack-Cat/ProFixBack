using Dapper;
using Domain.Types;
using System.Data;
using Repository.Repositories;

namespace Dal.SqlServer.Infrastructure;

public class SqlComplaintTypeRepository : IComplaintTypeRepository
{
    private readonly IDbConnection _dbConnection;

    public SqlComplaintTypeRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<IEnumerable<ComplaintType>> GetAllAsync()
    {
        var sql = "SELECT * FROM ComplaintType";
        return await _dbConnection.QueryAsync<ComplaintType>(sql);
    }
}
