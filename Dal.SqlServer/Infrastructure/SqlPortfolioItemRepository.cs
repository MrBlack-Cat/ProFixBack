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
    public class SqlPortfolioItemRepository : IPortfolioItemRepository
    {
        private readonly IDbConnection _dbConnection;

        public SqlPortfolioItemRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<PortfolioItem>> GetAllAsync()
        {
            var sql = "SELECT * FROM PortfolioItem WHERE IsDeleted = 0";
            return await _dbConnection.QueryAsync<PortfolioItem>(sql);
        }

        public async Task<PortfolioItem?> GetByIdAsync(int id)
        {
            var sql = "SELECT * FROM PortfolioItem WHERE Id = @Id AND IsDeleted = 0";
            return await _dbConnection.QueryFirstOrDefaultAsync<PortfolioItem>(sql, new { Id = id });
        }

        public async Task<IEnumerable<PortfolioItem>> GetByServiceProviderIdAsync(int serviceProviderProfileId)
        {
            var sql = "SELECT * FROM PortfolioItem WHERE ServiceProviderProfileId = @ServiceProviderProfileId AND IsDeleted = 0";
            return await _dbConnection.QueryAsync<PortfolioItem>(sql, new { ServiceProviderProfileId = serviceProviderProfileId });
        }

        public async Task AddAsync(PortfolioItem entity)
        {
            var sql = @"
                INSERT INTO PortfolioItem (ServiceProviderProfileId, Title, Description, ImageUrl, CreatedAt, CreatedBy)
                VALUES (@ServiceProviderProfileId, @Title, @Description, @ImageUrl, @CreatedAt, @CreatedBy)";
            await _dbConnection.ExecuteAsync(sql, entity);
        }

        public async Task UpdateAsync(PortfolioItem entity)
        {
            var sql = @"
                UPDATE PortfolioItem SET
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
                UPDATE PortfolioItem SET
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
