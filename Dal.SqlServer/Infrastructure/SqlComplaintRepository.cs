using Dapper;
using Domain.Entities;
using Repository.Repositories;
using System.Data;
using System.Security.Claims;

namespace Dal.SqlServer.Infrastructure
{
    public class SqlComplaintRepository : IComplaintRepository
    {
        private readonly IDbConnection _dbConnection;

        public SqlComplaintRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<Complaint>> GetAllAsync()
        {
            var sql = "SELECT * FROM Complaint WHERE IsDeleted = 0";
            return await _dbConnection.QueryAsync<Complaint>(sql);
        }

        public async Task<Complaint?> GetByIdAsync(int id)
        {
            var sql = "SELECT * FROM Complaint WHERE Id = @Id AND IsDeleted = 0";
            return await _dbConnection.QueryFirstOrDefaultAsync<Complaint>(sql, new { Id = id });
        }

        public async Task AddAsync(Complaint entity)
        {
            var sql = @"
                INSERT INTO Complaint (FromUserId, ToUserId, TypeId, Description, CreatedAt, CreatedBy)
                VALUES (@FromUserId, @ToUserId, @TypeId, @Description, @CreatedAt, @CreatedBy)";
            await _dbConnection.ExecuteAsync(sql, entity);
        }

        public async Task UpdateAsync(Complaint entity)
        {
            var sql = @"
                UPDATE Complaint SET
                    Description = @Description,
                    UpdatedAt = @UpdatedAt,
                    UpdatedBy = @UpdatedBy
                WHERE Id = @Id AND IsDeleted = 0";
            await _dbConnection.ExecuteAsync(sql, entity);
        }

        public async Task DeleteAsync(Complaint entity)
        {
            var sql = @"
                UPDATE Complaint SET 
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
            var complaint = await GetByIdAsync(id);
            if (complaint == null) return;

            int userId = int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            complaint.IsDeleted = true;
            complaint.DeletedAt = DateTime.UtcNow;
            complaint.DeletedBy = userId;
            complaint.DeletedReason = "Soft delete by Id";

            await DeleteAsync(complaint);
        }
    }
}
