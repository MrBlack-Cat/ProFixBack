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

namespace Dal.SqlServer.Infrastructure
{
    public class SqlActivityLogRepository : IActivityLogRepository
    {
        private readonly IDbConnection _dbConnection;

        public SqlActivityLogRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<ActivityLog>> GetAllAsync()
        {
            var sql = "SELECT * FROM ActivityLog WHERE IsDeleted = 0";
            return await _dbConnection.QueryAsync<ActivityLog>(sql);
        }

        public async Task<ActivityLog?> GetByIdAsync(int id)
        {
            var sql = "SELECT * FROM ActivityLog WHERE Id = @Id AND IsDeleted = 0";
            return await _dbConnection.QueryFirstOrDefaultAsync<ActivityLog>(sql, new { Id = id });
        }

        public async Task<IEnumerable<ActivityLog>> GetByUserIdAsync(int userId)
        {
            var sql = "SELECT * FROM ActivityLog WHERE UserId = @UserId AND IsDeleted = 0";
            return await _dbConnection.QueryAsync<ActivityLog>(sql, new { UserId = userId });
        }

        public async Task AddAsync(ActivityLog entity)
        {
            var sql = @"
                INSERT INTO ActivityLog (UserId, Action, EntityType, EntityId, CreatedAt, CreatedBy)
                VALUES (@UserId, @Action, @EntityType, @EntityId, @CreatedAt, @CreatedBy)";

            await _dbConnection.ExecuteAsync(sql, entity);
        }

        public async Task UpdateAsync(ActivityLog entity)
        {
            var sql = @"
                UPDATE ActivityLog SET
                    Action = @Action,
                    EntityType = @EntityType,
                    EntityId = @EntityId,
                    Timestamp = @Timestamp,
                    UpdatedAt = @UpdatedAt,
                    UpdatedBy = @UpdatedBy
                WHERE Id = @Id";
            await _dbConnection.ExecuteAsync(sql, entity);
        }

        public async Task DeleteAsync(ActivityLog entity)
        {
            var sql = @"
                UPDATE ActivityLog SET
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
}
