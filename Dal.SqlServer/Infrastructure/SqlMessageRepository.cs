using Dapper;
using Domain.Entities;
using Repository.Repositories;
using System.Data;
using System.Security.Claims;

public class SqlMessageRepository : IMessageRepository
{
    private readonly IDbConnection _dbConnection;

    public SqlMessageRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task AddAsync(Message entity)
    {
        var sql = @"
            INSERT INTO Message (SenderUserId, ReceiverUserId, Content, IsRead, CreatedAt, CreatedBy)
            VALUES (@SenderUserId, @ReceiverUserId, @Content, @IsRead, @CreatedAt, @CreatedBy)";
        await _dbConnection.ExecuteAsync(sql, entity);
    }

    public async Task DeleteAsync(Message entity)
    {
        var sql = @"
            UPDATE Message SET 
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


    public async Task<IEnumerable<Message>> GetAllAsync()
    {
        var sql = "SELECT * FROM Message WHERE IsDeleted = 0";
        return await _dbConnection.QueryAsync<Message>(sql);
    }

    public async Task<Message?> GetByIdAsync(int id)
    {
        var sql = "SELECT * FROM Message WHERE Id = @Id AND IsDeleted = 0";
        return await _dbConnection.QueryFirstOrDefaultAsync<Message>(sql, new { Id = id });
    }

    public async Task UpdateAsync(Message entity)
    {
        var sql = @"
            UPDATE Message SET 
                Content = @Content,
                IsRead = @IsRead,
                UpdatedAt = @UpdatedAt,
                UpdatedBy = @UpdatedBy
            WHERE Id = @Id";
        await _dbConnection.ExecuteAsync(sql, entity);
    }

    public async Task<IEnumerable<Message>> GetByUserIdAsync(int userId)
    {
        var sql = @"
            SELECT * FROM Message
            WHERE (SenderUserId = @UserId OR ReceiverUserId = @UserId) AND IsDeleted = 0";
        return await _dbConnection.QueryAsync<Message>(sql, new { UserId = userId });
    }

    public async Task<IEnumerable<Message>> GetAllBetweenUsersAsync(int userId1, int userId2)
    {
        var sql = @"
            SELECT * FROM Message
            WHERE 
                ((SenderUserId = @UserId1 AND ReceiverUserId = @UserId2) OR
                 (SenderUserId = @UserId2 AND ReceiverUserId = @UserId1))
                AND IsDeleted = 0
            ORDER BY CreatedAt ASC";
        return await _dbConnection.QueryAsync<Message>(sql, new { UserId1 = userId1, UserId2 = userId2 });
    }

    public async Task<IEnumerable<dynamic>> GetRawChatSummariesByUserIdAsync(int userId)
    {
        var sql = @"
                WITH LastMessages AS (
                    SELECT *,
                           ROW_NUMBER() OVER (
                               PARTITION BY 
                                   CASE 
                                       WHEN SenderUserId < ReceiverUserId THEN CONCAT(SenderUserId, '_', ReceiverUserId)
                                       ELSE CONCAT(ReceiverUserId, '_', SenderUserId)
                                   END
                               ORDER BY CreatedAt DESC
                           ) AS rn
                    FROM Message m
                    WHERE (SenderUserId = @UserId OR ReceiverUserId = @UserId) AND m.IsDeleted = 0
                )
                SELECT 
                    CASE 
                        WHEN SenderUserId = @UserId THEN ReceiverUserId 
                        ELSE SenderUserId 
                    END AS OtherUserId,
                    u.UserName AS OtherUserName,
                    Content AS LastMessageContent,
                    m.CreatedAt AS LastMessageTime
                FROM LastMessages m
                JOIN Users u ON u.Id = CASE 
                    WHEN m.SenderUserId = @UserId THEN m.ReceiverUserId 
                    ELSE m.SenderUserId 
                END
                WHERE rn = 1
                ORDER BY m.CreatedAt DESC;
            ";

        return await _dbConnection.QueryAsync(sql, new { UserId = userId });
    }

    public async Task<int> GetUnreadCountByUserIdAsync(int userId)
    {
        const string sql = @"
        SELECT COUNT(*)
        FROM Message
        WHERE ReceiverUserId = @UserId AND IsRead = 0 AND IsDeleted = 0;
    ";

        return await _dbConnection.ExecuteScalarAsync<int>(sql, new { UserId = userId });
    }

    public async Task<IEnumerable<Message>> GetAllMessagesAsync()
    {
        var sql = "SELECT * FROM Message WHERE IsDeleted = 0";
        return await _dbConnection.QueryAsync<Message>(sql);
    }

}
