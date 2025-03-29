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
    public class SqlCertificateRepository : ICertificateRepository
    {
        private readonly IDbConnection _dbConnection;

        public SqlCertificateRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<Certificate>> GetAllAsync()
        {
            var sql = "SELECT * FROM Certificate WHERE IsDeleted = 0";
            return await _dbConnection.QueryAsync<Certificate>(sql);
        }

        public async Task<Certificate?> GetByIdAsync(int id)
        {
            var sql = "SELECT * FROM Certificate WHERE Id = @Id AND IsDeleted = 0";
            return await _dbConnection.QueryFirstOrDefaultAsync<Certificate>(sql, new { Id = id });
        }

        public async Task<IEnumerable<Certificate>> GetByServiceProviderIdAsync(int serviceProviderProfileId)
        {
            var sql = "SELECT * FROM Certificate WHERE ServiceProviderProfileId = @ServiceProviderProfileId AND IsDeleted = 0";
            return await _dbConnection.QueryAsync<Certificate>(sql, new { ServiceProviderProfileId = serviceProviderProfileId });
        }

        public async Task AddAsync(Certificate entity)
        {
            var sql = @"
                INSERT INTO Certificate (ServiceProviderProfileId, Title, Description, FileUrl, IssuedAt, CreatedAt, CreatedBy)
                VALUES (@ServiceProviderProfileId, @Title, @Description, @FileUrl, @IssuedAt, @CreatedAt, @CreatedBy)";
            await _dbConnection.ExecuteAsync(sql, entity);
        }

        public async Task UpdateAsync(Certificate entity)
        {
            var sql = @"
                UPDATE Certificate SET
                    Title = @Title,
                    Description = @Description,
                    FileUrl = @FileUrl,
                    IssuedAt = @IssuedAt,
                    UpdatedAt = @UpdatedAt,
                    UpdatedBy = @UpdatedBy
                WHERE Id = @Id";
            await _dbConnection.ExecuteAsync(sql, entity);
        }

        public async Task DeleteAsync(Certificate entity)
        {
            var sql = @"
                UPDATE Certificate SET
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
