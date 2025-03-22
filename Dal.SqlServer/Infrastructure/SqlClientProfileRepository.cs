using Dapper;
using Domain.Entities;
using Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Dal.SqlServer.Infrastructure;

public class SqlClientProfileRepository : IClientProfileRepository
{
    private readonly IDbConnection _dbConnection;

    public SqlClientProfileRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<IEnumerable<ClientProfile>> GetAllAsync()
    {
        var sql = "SELECT * FROM ClientProfiles WHERE IsDeleted = 0";
        return await _dbConnection.QueryAsync<ClientProfile>(sql);
    }

    public async Task<ClientProfile?> GetByIdAsync(int id)
    {
        var sql = "SELECT * FROM ClientProfiles WHERE Id = @Id AND IsDeleted = 0";
        return await _dbConnection.QueryFirstOrDefaultAsync<ClientProfile>(sql, new { Id = id });
    }

    public async Task AddAsync(ClientProfile entity)
    {
        var sql = @"
                INSERT INTO ClientProfiles (UserId, FullName, City, AvatarUrl, About, OtherContactLinks, CreatedAt, CreatedBy)
                VALUES (@UserId, @FullName, @City, @AvatarUrl, @About, @OtherContactLinks, @CreatedAt, @CreatedBy)";
        await _dbConnection.ExecuteAsync(sql, entity);
    }

    public async Task UpdateAsync(ClientProfile entity)
    {
        var sql = @"
                UPDATE ClientProfiles SET
                    FullName = @FullName,
                    City = @City,
                    AvatarUrl = @AvatarUrl,
                    About = @About,
                    OtherContactLinks = @OtherContactLinks,
                    UpdatedAt = @UpdatedAt,
                    UpdatedBy = @UpdatedBy
                WHERE Id = @Id";
        await _dbConnection.ExecuteAsync(sql, entity);
    }

    public async Task DeleteAsync(ClientProfile entity)
    {
        var sql = @"
                UPDATE ClientProfiles SET 
                    IsDeleted = 1,
                    DeletedAt = @DeletedAt,
                    DeletedBy = @DeletedBy,
                    DeleteReason = @DeleteReason
                WHERE Id = @Id";

        await _dbConnection.ExecuteAsync(sql, new
        {
            entity.Id,
            DeletedAt = entity.DeletedAt ?? DateTime.UtcNow,
            entity.DeletedBy,
            entity.DeletedReason
        });
    }

    public async Task DeleteAsync(int id , ClaimsPrincipal user)
    {
        var profile = await GetByIdAsync(id);
        if (profile == null) return;

        int userId = int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

        profile.IsDeleted = true;
        profile.DeletedAt = DateTime.UtcNow;
        profile.DeletedBy = userId;
        profile.DeletedReason = "Soft delete by Id";

        await DeleteAsync(profile);
    }
}
