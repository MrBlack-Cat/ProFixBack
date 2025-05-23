﻿using Dapper;
using Domain.Entities;
using Repository.Repositories;
using System.Data;
using System.Security.Claims;

namespace Dal.SqlServer.Infrastructure;

public class SqlPostLikeRepository : IPostLikeRepository
{
    private readonly IDbConnection _db;

    public SqlPostLikeRepository(IDbConnection db)
    {
        _db = db;
    }

    public async Task<IEnumerable<PostLike>> GetAllPostLikesAsync()
    {
        const string sql = @"SELECT Id, PostId, ClientProfileId, CreatedAt FROM PostLikes";
        var likes = await _db.QueryAsync<PostLike>(sql);
        return likes;
    }

    public async Task<bool> HasLikedAsync(int postId, int clientProfileId)
    {
        var sql = @"SELECT COUNT(1) FROM PostLikes WHERE PostId = @postId AND ClientProfileId = @clientProfileId";
        var count = await _db.ExecuteScalarAsync<int>(sql, new { postId, clientProfileId });
        return count > 0;
    }

    public async Task<int> GetLikesCountAsync(int postId)
    {
        var sql = @"SELECT COUNT(*) FROM PostLikes WHERE PostId = @postId";
        return await _db.ExecuteScalarAsync<int>(sql, new { postId });
    }

    public async Task AddLikeAsync(int postId, int clientProfileId)
    {
        var sql = @"INSERT INTO PostLikes (PostId, ClientProfileId, CreatedAt) VALUES (@postId, @clientProfileId, @now)";
        await _db.ExecuteAsync(sql, new { postId, clientProfileId, now = DateTime.UtcNow });
    }

    public async Task RemoveLikeAsync(int postId, int clientProfileId)
    {
        var sql = @"DELETE FROM PostLikes WHERE PostId = @postId AND ClientProfileId = @clientProfileId";
        await _db.ExecuteAsync(sql, new { postId, clientProfileId });
    }



    public Task<IEnumerable<PostLike>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    //Istifade olunmayanlar

    public Task<PostLike?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task AddAsync(PostLike entity)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(PostLike entity)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(int id, ClaimsPrincipal user)
    {
        throw new NotImplementedException(); 
    }

    public Task DeleteAsync(PostLike entity)
    {
        throw new NotImplementedException(); 
    }
}
