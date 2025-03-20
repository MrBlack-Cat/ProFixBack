using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Repository.Common;

namespace DAL.SqlServer.Infrastructure
{
    public abstract class BaseSqlRepository<T> : IRepository<T> where T : class
    {
        protected readonly IDbConnection _dbConnection;

        public BaseSqlRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            var query = $"SELECT * FROM [{typeof(T).Name}] WHERE IsDeleted = 0";
            return await _dbConnection.QueryAsync<T>(query);
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            var query = $"SELECT * FROM [{typeof(T).Name}] WHERE Id = @Id AND IsDeleted = 0";
            return await _dbConnection.QueryFirstOrDefaultAsync<T>(query, new { Id = id });
        }

        public virtual Task AddAsync(T entity)
        {
            throw new System.NotImplementedException("Use specific repository for INSERT logic.");
        }

        public virtual Task UpdateAsync(T entity)
        {
            throw new System.NotImplementedException("Use specific repository for UPDATE logic.");
        }

        public virtual async Task DeleteAsync(int id)
        {
            var query = $@"
                UPDATE [{typeof(T).Name}]
                SET IsDeleted = 1, DeletedAt = GETUTCDATE(), DeletedBy = 'System', DeleteReason = 'Soft delete by ID'
                WHERE Id = @Id";

            await _dbConnection.ExecuteAsync(query, new { Id = id });
        }

        public virtual async Task DeleteAsync(T entity)
        {
            var query = $@"
                UPDATE [{typeof(T).Name}]
                SET IsDeleted = 1, DeletedAt = GETUTCDATE(), DeletedBy = 'System', DeleteReason = 'Soft delete'
                WHERE Id = @Id";

            var entityType = typeof(T);
            var property = entityType.GetProperty("Id");

            if (property == null)
                throw new System.Exception("Entity must have Id property");

            var idValue = property.GetValue(entity);
            await _dbConnection.ExecuteAsync(query, new { Id = idValue });
        }
    }
}
