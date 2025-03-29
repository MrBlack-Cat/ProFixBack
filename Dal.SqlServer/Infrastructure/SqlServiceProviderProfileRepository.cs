using Dapper;
using Domain.Entities;
using Repository.Repositories;
using System.Data;
using System.Security.Claims;

public class SqlServiceProviderProfileRepository : IServiceProviderProfileRepository
{
    private readonly IDbConnection _dbConnection;

    public SqlServiceProviderProfileRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<IEnumerable<ServiceProviderProfile>> GetAllAsync()
    {
        var sql = "SELECT * FROM ServiceProviderProfile WHERE IsDeleted = 0";
        return await _dbConnection.QueryAsync<ServiceProviderProfile>(sql);
    }

    public async Task<ServiceProviderProfile?> GetByIdAsync(int id)
    {
        var sql = "SELECT * FROM ServiceProviderProfile WHERE Id = @Id AND IsDeleted = 0";
        return await _dbConnection.QueryFirstOrDefaultAsync<ServiceProviderProfile>(sql, new { Id = id });
    }

    public async Task AddAsync(ServiceProviderProfile entity)
    {
        var sql = @"
            INSERT INTO ServiceProviderProfiles (UserId, Name, Surname, City, Age, GenderId, ExperienceYears, Description, IsApprovedByAdmin, CreatedAt, CreatedBy)
            VALUES (@UserId, @Name, @Surname, @City, @Age, @GenderId, @ExperienceYears, @Description, @IsApprovedByAdmin, @CreatedAt, @CreatedBy)";
        await _dbConnection.ExecuteAsync(sql, entity);
    }

    public async Task UpdateAsync(ServiceProviderProfile entity)
    {
        var sql = @"
            UPDATE ServiceProviderProfiles SET
                Name = @Name,
                Surname = @Surname,
                City = @City,
                Age = @Age,
                GenderId = @GenderId,
                ExperienceYears = @ExperienceYears,
                Description = @Description,
                IsApprovedByAdmin = @IsApprovedByAdmin,
                ApprovalDate = @ApprovalDate,
                UpdatedAt = @UpdatedAt,
                UpdatedBy = @UpdatedBy
            WHERE Id = @Id";
        await _dbConnection.ExecuteAsync(sql, entity);
    }

    public async Task DeleteAsync(ServiceProviderProfile entity)
    {
        var sql = @"
            UPDATE ServiceProviderProfile SET 
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
        var profile = await GetByIdAsync(id);
        if (profile == null) return;

        int userId = int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

        profile.IsDeleted = true;
        profile.DeletedAt = DateTime.UtcNow;
        profile.DeletedBy = userId;
        profile.DeletedReason = "Soft delete by Id";

        await DeleteAsync(profile);
    }

    public async Task<IEnumerable<ServiceProviderProfile>> GetByCityAsync(string city)
    {
        var sql = "SELECT * FROM ServiceProviderProfile WHERE City = @City AND IsDeleted = 0";
        return await _dbConnection.QueryAsync<ServiceProviderProfile>(sql, new { City = city });
    }

    public async Task<IEnumerable<ServiceProviderProfile>> GetApprovedAsync()
    {
        var sql = "SELECT * FROM ServiceProviderProfile WHERE IsApprovedByAdmin = 1 AND IsDeleted = 0";
        return await _dbConnection.QueryAsync<ServiceProviderProfile>(sql);
    }

    public async Task<ServiceProviderProfile?> GetByUserIdAsync(int userId)
    {
        var sql = "SELECT * FROM ServiceProviderProfile WHERE UserId = @UserId AND IsDeleted = 0";
        return await _dbConnection.QueryFirstOrDefaultAsync<ServiceProviderProfile>(sql, new { UserId = userId });
    }

}
