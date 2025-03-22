using Dapper;
using Domain.Entities;
using Repository.Repositories;
using System.Data;
using System.Security.Claims;

namespace DAL.SqlServer.Infrastructure
{
    public class SqlUserRepository : IUserRepository
    {
        private readonly IDbConnection _dbConnection;

        public SqlUserRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            var sql = "SELECT * FROM Users WHERE IsDeleted = 0";
            return await _dbConnection.QueryAsync<User>(sql);
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            var sql = "SELECT * FROM Users WHERE Id = @Id AND IsDeleted = 0";
            return await _dbConnection.QueryFirstOrDefaultAsync<User>(sql, new { Id = id });
        }

        public async Task AddAsync(User entity)
        {
            var sql = @"
                INSERT INTO Users (Email, PhoneNumber, PasswordHash, RoleId, IsActive, CreatedAt, CreatedBy)
                VALUES (@Email, @PhoneNumber, @PasswordHash, @RoleId, @IsActive, @CreatedAt, @CreatedBy)";
            await _dbConnection.ExecuteAsync(sql, entity);
        }

        public async Task UpdateAsync(User entity)
        {
            var sql = @"
                UPDATE Users SET 
                    Email = @Email,
                    PhoneNumber = @PhoneNumber,
                    PasswordHash = @PasswordHash,
                    RoleId = @RoleId,
                    IsActive = @IsActive,
                    UpdatedAt = @UpdatedAt,
                    UpdatedBy = @UpdatedBy,
                    IsDeleted = @IsDeleted,
                    DeletedAt = @DeletedAt,
                    DeletedBy = @DeletedBy,
                    DeleteReason = @DeleteReason
                WHERE Id = @Id";
            await _dbConnection.ExecuteAsync(sql, entity);
        }

        public async Task DeleteAsync(User entity)
        {
            var sql = @"
                UPDATE Users SET 
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

        //deyishiklik olunub claimsPrincipal ile 
        public async Task DeleteAsync(int id , ClaimsPrincipal user1)
        {
            var user = await GetByIdAsync(id);
            if (user == null) return;

            int userId = int.Parse(user1.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            user.IsDeleted = true;
            user.DeletedAt = DateTime.UtcNow;
            user.DeletedBy = userId;
            user.DeletedReason = "Soft delete by Id";

            await DeleteAsync(user);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            var sql = "SELECT * FROM Users WHERE Email = @Email AND IsDeleted = 0";
            return await _dbConnection.QueryFirstOrDefaultAsync<User>(sql, new { Email = email });
        }

        public async Task<User?> GetByUserNameAsync(string username)
        {
            var sql = "SELECT * FROM Users WHERE UserName = @UserName AND IsDeleted = 0";
            return await _dbConnection.QueryFirstOrDefaultAsync<User>(sql, new { UserName = username });

            throw new NotImplementedException("UserName field not implemented in Users table");
        }
    }
}
