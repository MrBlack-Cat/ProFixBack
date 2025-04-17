using Dapper;
using Domain.Entities;
using Repository.Repositories;
using System.Data;
using System.Security.Claims;

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
        var sql = "SELECT * FROM ClientProfile WHERE IsDeleted = 0";
        return await _dbConnection.QueryAsync<ClientProfile>(sql);
    }

    public async Task<ClientProfile?> GetByIdAsync(int id)
    {
        var sql = "SELECT * FROM ClientProfile WHERE Id = @Id AND IsDeleted = 0";
        return await _dbConnection.QueryFirstOrDefaultAsync<ClientProfile>(sql, new { Id = id });
    }

    public async Task AddAsync(ClientProfile entity)
    {
        var sql = @"
            INSERT INTO ClientProfile 
                (UserId, Name, Surname, City, AvatarUrl, About, OtherContactLinks, CreatedAt, CreatedBy)
            VALUES 
                (@UserId, @Name, @Surname, @City, @AvatarUrl, @About, @OtherContactLinks, @CreatedAt, @CreatedBy)";
        await _dbConnection.ExecuteAsync(sql, entity);
    }

    public async Task UpdateAsync(ClientProfile entity)
    {
        var sql = @"
            UPDATE ClientProfile SET
                Name = @Name,
                Surname = @Surname,
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
            UPDATE ClientProfile SET 
                IsDeleted = 1,
                DeletedAt = @DeletedAt,
                DeletedBy = @DeletedBy,
                DeletedReason = @DeletedReason
            WHERE Id = @Id";

        await _dbConnection.ExecuteAsync(sql, new
        {
            entity.Id,
            DeletedAt = entity.DeletedAt ?? DateTime.UtcNow,
            entity.DeletedBy,
            entity.DeletedReason
        });
    }

    public async Task DeleteAsync(int id, ClaimsPrincipal user)
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

    public async Task<ClientProfile?> GetByUserIdAsync(int userId)
    {
        var sql = "SELECT * FROM ClientProfile WHERE UserId = @UserId AND IsDeleted = 0";
        return await _dbConnection.QueryFirstOrDefaultAsync<ClientProfile>(sql, new { UserId = userId });
    }

    public async Task<(string Name, string Surname)?> GetNameSurnameByUserIdAsync(int userId)
    {
        var sql = "SELECT Name, Surname FROM ClientProfile WHERE UserId = @UserId";
        return await _dbConnection.QueryFirstOrDefaultAsync<(string, string)?>(sql, new { UserId = userId });
    }

}
