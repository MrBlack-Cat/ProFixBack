using Dapper;
using Domain.Entities;
using Repository.Repositories;
using System.Data;
using System.Security.Claims;

namespace Dal.SqlServer.Infrastructure;

public class SqlNotificationRepository : INotificationRepository
{
    private readonly IDbConnection _dbConnection;

    public SqlNotificationRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<IEnumerable<Notification>> GetAllAsync()
    {
        var sql = "SELECT * FROM Notification WHERE IsDeleted = 0";
        return await _dbConnection.QueryAsync<Notification>(sql);
    }

    public async Task<Notification?> GetByIdAsync(int id)
    {
        var sql = "SELECT * FROM Notification WHERE Id = @Id AND IsDeleted = 0";
        return await _dbConnection.QueryFirstOrDefaultAsync<Notification>(sql, new { Id = id });
    }

    public async Task<IEnumerable<Notification>> GetByUserIdAsync(int userId)
    {
        var sql = "SELECT * FROM Notification WHERE UserId = @UserId AND IsDeleted = 0";
        return await _dbConnection.QueryAsync<Notification>(sql, new { UserId = userId });
    }

    public async Task<List<Notification>> GetUnreadByUserIdAsync(int userId)
    {
        var sql = "SELECT * FROM Notification WHERE UserId = @UserId AND IsDeleted = 0 AND IsRead = 0";
        var result = await _dbConnection.QueryAsync<Notification>(sql, new { UserId = userId });
        return result.ToList();
    }


    public async Task AddAsync(Notification entity)
    {
        var sql = @"
            INSERT INTO Notification (UserId, TypeId, Message, IsRead, CreatedAt, CreatedBy)
            VALUES (@UserId, @TypeId, @Message, @IsRead, @CreatedAt, @CreatedBy)";
        await _dbConnection.ExecuteAsync(sql, entity);
    }

    public async Task UpdateAsync(Notification entity)
    {
        var sql = @"
            UPDATE Notification SET
                TypeId = @TypeId,
                Message = @Message,
                IsRead = @IsRead,
                UpdatedAt = @UpdatedAt,
                UpdatedBy = @UpdatedBy
            WHERE Id = @Id";
        await _dbConnection.ExecuteAsync(sql, entity);
    }

    public async Task DeleteAsync(Notification entity)
    {
        var sql = @"
            UPDATE Notification SET
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
        var entity = await GetByIdAsync(id);
        if (entity == null) return;

        int userId = int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        entity.DeletedBy = userId;
        entity.DeletedReason = "Soft delete by Id";

        await DeleteAsync(entity);
    }
}
