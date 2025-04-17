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
    public class SqlGuaranteeDocumentRepository : IGuaranteeDocumentRepository
    {
        private readonly IDbConnection _dbConnection;

        public SqlGuaranteeDocumentRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<GuaranteeDocument>> GetAllAsync()
        {
            var sql = "SELECT * FROM GuaranteeDocument WHERE IsDeleted = 0";
            return await _dbConnection.QueryAsync<GuaranteeDocument>(sql);
        }

        public async Task<GuaranteeDocument?> GetByIdAsync(int id)
        {
            var sql = "SELECT * FROM GuaranteeDocument WHERE Id = @Id AND IsDeleted = 0";
            return await _dbConnection.QueryFirstOrDefaultAsync<GuaranteeDocument>(sql, new { Id = id });
        }

        public async Task<IEnumerable<GuaranteeDocument>> GetByClientIdAsync(int clientProfileId)
        {
            var sql = "SELECT * FROM GuaranteeDocument WHERE ClientProfileId = @ClientProfileId AND IsDeleted = 0";
            return await _dbConnection.QueryAsync<GuaranteeDocument>(sql, new { ClientProfileId = clientProfileId });
        }

        public async Task<IEnumerable<GuaranteeDocument>> GetByServiceProviderIdAsync(int serviceProviderProfileId)
        {
            var sql = "SELECT * FROM GuaranteeDocument WHERE ServiceProviderProfileId = @ServiceProviderProfileId AND IsDeleted = 0";
            return await _dbConnection.QueryAsync<GuaranteeDocument>(sql, new { ServiceProviderProfileId = serviceProviderProfileId });
        }

        public async Task AddAsync(GuaranteeDocument entity)
        {
            var sql = @"
                    INSERT INTO GuaranteeDocument 
                    (ServiceProviderProfileId, ClientProfileId, Title, Description, FileUrl, CreatedAt, CreatedBy, IssueDate, ExpirationDate)
                    VALUES 
                    (@ServiceProviderProfileId, @ClientProfileId, @Title, @Description, @FileUrl, @CreatedAt, @CreatedBy, @IssueDate, @ExpirationDate)";

            await _dbConnection.ExecuteAsync(sql, entity);
        }

        public async Task UpdateAsync(GuaranteeDocument entity)
        {
            var sql = @"
                    UPDATE GuaranteeDocument SET
                        Title = @Title,
                        Description = @Description,
                        FileUrl = @FileUrl,
                        UpdatedAt = @UpdatedAt,
                        UpdatedBy = @UpdatedBy,
                        IssueDate = @IssueDate,
                        ExpirationDate = @ExpirationDate
                    WHERE Id = @Id";

            await _dbConnection.ExecuteAsync(sql, entity);
        }


        public async Task DeleteAsync(GuaranteeDocument entity)
        {
            var sql = @"
                UPDATE GuaranteeDocument SET
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
