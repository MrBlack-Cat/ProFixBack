﻿using Dapper;
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
    public class SqlPostRepository : IPostRepository
    {
        private readonly IDbConnection _dbConnection;

        public SqlPostRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<Post>> GetAllAsync()
        {
            var sql = "SELECT * FROM Post WHERE IsDeleted = 0";
            return await _dbConnection.QueryAsync<Post>(sql);
        }

        public async Task<Post?> GetByIdAsync(int id)
        {
            var sql = "SELECT * FROM Post WHERE Id = @Id AND IsDeleted = 0";
            return await _dbConnection.QueryFirstOrDefaultAsync<Post>(sql, new { Id = id });
        }

        public async Task<IEnumerable<Post>> GetByServiceProviderIdAsync(int serviceProviderProfileId)
        {
            var sql = "SELECT * FROM Post WHERE ServiceProviderProfileId = @ServiceProviderProfileId AND IsDeleted = 0";
            return await _dbConnection.QueryAsync<Post>(sql, new { ServiceProviderProfileId = serviceProviderProfileId });
        }

        public async Task AddAsync(Post entity)
        {
            var sql = @"
                INSERT INTO Post (ServiceProviderProfileId, Title, Content, ImageUrl, CreatedAt, CreatedBy)
                VALUES (@ServiceProviderProfileId, @Title, @Content, @ImageUrl, @CreatedAt, @CreatedBy)";
            await _dbConnection.ExecuteAsync(sql, entity);
        }

        public async Task UpdateAsync(Post entity)
        {
            var sql = @"
                UPDATE Post SET
                    Title = @Title,
                    Content = @Content,
                    ImageUrl = @ImageUrl,
                    UpdatedAt = @UpdatedAt,
                    UpdatedBy = @UpdatedBy
                WHERE Id = @Id";
            await _dbConnection.ExecuteAsync(sql, entity);
        }

        public async Task DeleteAsync(Post entity)
        {
            var sql = @"
                UPDATE Post SET
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

        public async Task<IEnumerable<Post>> GetPostsByProviderIdAsync(int serviceProviderProfileId)
        {
            var query = @"
                        SELECT Id, ServiceProviderProfileId, Title, Content, ImageUrl, CreatedBy, CreatedAt
                        FROM Post
                        WHERE ServiceProviderProfileId = @ServiceProviderProfileId AND IsDeleted = 0
                        ORDER BY CreatedAt DESC;
                    ";

            var result = await _dbConnection.QueryAsync<Post>(query, new { ServiceProviderProfileId = serviceProviderProfileId });
            return result;
        }

        public async Task<IEnumerable<Post>> GetPostsByLikedAsync()
        {
            var sql = @"
                SELECT p.Id, p.ServiceProviderProfileId, p.Title, p.Content, p.ImageUrl, p.CreatedBy, p.CreatedAt
                FROM Post p
                WHERE p.Id IN (
                    SELECT PostId FROM PostLikes
                )
                AND p.IsDeleted = 0
                ORDER BY p.CreatedAt DESC
            ";

            return await _dbConnection.QueryAsync<Post>(sql);
        }

        public async Task<IEnumerable<Post>> GetPostsByUserIdAsync(int userId)
        {
            const string sql = @"
                SELECT p.*
                FROM Post p
                INNER JOIN ServiceProviderProfile spp ON p.ServiceProviderProfileId = spp.Id
                WHERE spp.UserId = @UserId AND p.IsDeleted = 0;
            ";

            return await _dbConnection.QueryAsync<Post>(sql, new { UserId = userId });
        }


        public async Task<int> GetTotalLikesByUserIdAsync(int userId)
        {
            const string sql = @"
                SELECT COUNT(pl.PostId)
                FROM PostLikes pl
                INNER JOIN Post p ON pl.PostId = p.Id
                INNER JOIN ServiceProviderProfile spp ON p.ServiceProviderProfileId = spp.Id
                WHERE spp.UserId = @UserId AND p.IsDeleted = 0;
            ";

            return await _dbConnection.ExecuteScalarAsync<int>(sql, new { UserId = userId });
        }
        public async Task<IEnumerable<Post>> GetAllPostsAsync()
        {
            var sql = "SELECT * FROM Post WHERE IsDeleted = 0";
            return await _dbConnection.QueryAsync<Post>(sql);
        }

    }
}
