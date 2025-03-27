using Dapper;
using Domain.Entities;
using System.Data;

public class SqlUserRoleRepository : IUserRoleRepository
{
    private readonly IDbConnection _dbConnection;

    public SqlUserRoleRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<IEnumerable<UserRole>> GetAllRolesAsync()
    {
        var sql = "SELECT * FROM UserRole";
        return await _dbConnection.QueryAsync<UserRole>(sql);
    }
}
