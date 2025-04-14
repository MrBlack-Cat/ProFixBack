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
}
