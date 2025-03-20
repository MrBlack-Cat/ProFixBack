using Dapper;
using Domain.Entities;
using Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal.SqlServer.Infrastructure
{
    public class SqlPortfolioItemRepository : IPortfolioItemRepository
    {
        private readonly IDbConnection _dbConnection;

        public SqlPortfolioItemRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<PortfolioItem>> GetAllAsync()
        {
            var sql = "SELECT * FROM PortfolioItems WHERE IsDeleted = 0";
            return await _dbConnection.QueryAsync<PortfolioItem>(sql);
        }

        public async Task<PortfolioItem?> GetByIdAsync(int id)
        {
            var sql = "SELECT * FROM PortfolioItems WHERE Id = @Id AND IsDeleted = 0";
            return await _dbConnection.QueryFirstOrDefaultAsync<PortfolioItem>(sql, new { Id = id });
        }

        public async Task<IEnumerable<PortfolioItem>> GetByServiceProviderIdAsync(int serviceProviderProfileId)
        {
            var sql = "SELECT * FROM PortfolioItems WHERE ServiceProviderProfileId = @ServiceProviderProfileId AND IsDeleted = 0";
            return await _dbConnection.QueryAsync<PortfolioItem>(sql, new { ServiceProviderProfileId = serviceProviderProfileId });
        }

        public async Task AddAsync(PortfolioItem entity)
        {
            var sql = @"
                INSERT INTO PortfolioItems (ServiceProviderProfileId, Title, Description, ImageUrl, CreatedAt, CreatedBy)
                VALUES (@ServiceProviderProfileId, @Title, @Description, @ImageUrl, @CreatedAt, @CreatedBy)";
            await _dbConnection.ExecuteAsync(sql, entity);
        }

        public async Task UpdateAsync(PortfolioItem entity)
        {
            var sql = @"
                UPDATE PortfolioItems SET
                    Title = @Title,
                    Description = @Description,
                    ImageUrl = @ImageUrl,
                    UpdatedAt = @UpdatedAt,
                    UpdatedBy = @UpdatedBy
                WHERE Id = @Id";
            await _dbConnection.ExecuteAsync(sql, entity);
        }

        public async Task DeleteAsync(PortfolioItem entity)
        {
            var sql = @"
                UPDATE PortfolioItems SET
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

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null) return;

            entity.IsDeleted = true;
            entity.DeletedAt = DateTime.UtcNow;
            entity.DeletedBy = "System";
            entity.DeletedReason = "Soft delete by Id";

            await DeleteAsync(entity);
        }
    }
}
