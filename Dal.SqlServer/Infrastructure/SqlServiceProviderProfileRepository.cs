using Dapper;
using Domain.Entities;
using Domain.Types;
using Microsoft.EntityFrameworkCore;
using Repository.Repositories;
using System.Data;
using System.Reflection;
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

    //public async Task<ServiceProviderProfile?> GetByIdAsync(int id)
    //{
    //    var sql = "SELECT * FROM ServiceProviderProfile WHERE Id = @Id AND IsDeleted = 0";
    //    return await _dbConnection.QueryFirstOrDefaultAsync<ServiceProviderProfile>(sql, new { Id = id });
    //}


    public async Task<ServiceProviderProfile?> GetByIdAsync(int id)
    {
        var sql = @"
            SELECT 
                sp.*, 
                g.Name AS GenderName,
                pc.Name AS ParentCategoryName
            FROM ServiceProviderProfile sp
            LEFT JOIN GenderType g ON sp.GenderId = g.Id
            LEFT JOIN ParentCategory pc ON sp.ParentCategoryId = pc.Id
            WHERE sp.Id = @Id AND sp.IsDeleted = 0;

            SELECT st.Name 
            FROM ServiceProviderServiceTypes sst
            JOIN ServiceType st ON sst.ServiceTypeId = st.Id
            WHERE sst.ServiceProviderProfileId = @Id;
            ";

        using var multi = await _dbConnection.QueryMultipleAsync(sql, new { Id = id });

        var profile = await multi.ReadFirstOrDefaultAsync<ServiceProviderProfile>();
        if (profile == null)
            return null;

        var serviceTypes = (await multi.ReadAsync<string>()).ToList();

        profile.ServiceTypes = serviceTypes;

        return profile;
    }


    public async Task AddAsync(ServiceProviderProfile entity)
    {
        var sql = @"
    INSERT INTO ServiceProviderProfile 
        (UserId, Name, Surname, City, Age, GenderId, ExperienceYears, Description, AvatarUrl, IsApprovedByAdmin, CreatedAt, CreatedBy, ParentCategoryId)
    OUTPUT INSERTED.Id
    VALUES 
        (@UserId, @Name, @Surname, @City, @Age, @GenderId, @ExperienceYears, @Description, @AvatarUrl, @IsActive, @CreatedAt, @CreatedBy, @ParentCategoryId);";


        var id = await _dbConnection.ExecuteScalarAsync<int>(sql, entity);
        entity.Id = id; 
    }


    public async Task UpdateAsync(ServiceProviderProfile entity)
    {
        var sql = @"
        UPDATE ServiceProviderProfile SET
            Name = @Name,
            Surname = @Surname,
            City = @City,
            Age = @Age,
            GenderId = @GenderId,
            ExperienceYears = @ExperienceYears,
            Description = @Description,
            AvatarUrl = @AvatarUrl,
            ParentCategoryId = @ParentCategoryId,
            IsApprovedByAdmin = @IsActive,
            ApprovalDate = @ApprovalDate,
            UpdatedAt = @UpdatedAt,
            UpdatedBy = @UpdatedBy
        WHERE Id = @Id";

        await _dbConnection.ExecuteAsync(sql, entity);

        // ❗ ВАЖНО: Используем @Id, и передаём { Id = ... }
        await _dbConnection.ExecuteAsync(
            "DELETE FROM ServiceProviderServiceTypes WHERE ServiceProviderProfileId = @ServiceProviderProfileId",
            new { ServiceProviderProfileId = entity.Id });


        foreach (var serviceTypeId in entity.ServiceTypeIds)
        {
            await _dbConnection.ExecuteAsync(@"
            INSERT INTO ServiceProviderServiceTypes (ServiceProviderProfileId, ServiceTypeId)
            VALUES (@ProfileId, @ServiceTypeId)",
                new { ProfileId = entity.Id, ServiceTypeId = serviceTypeId });
        }
    }


    public async Task DeleteAsync(ServiceProviderProfile entity)
    {
        var sql = @"
            UPDATE ServiceProviderProfile SET 
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

    //public async Task<ServiceProviderProfile?> GetByUserIdAsync(int userId)
    //{
    //    var sql = "SELECT * FROM ServiceProviderProfile WHERE UserId = @UserId AND IsDeleted = 0";
    //    return await _dbConnection.QueryFirstOrDefaultAsync<ServiceProviderProfile>(sql, new { UserId = userId });
    //}


    //public async Task<ServiceProviderProfile?> GetByUserIdAsync(int userId)
    //{
    //    var sql = @"
    //        SELECT 
    //            sp.*, 
    //            g.Name AS GenderName,
    //            pc.Name AS ParentCategoryName
    //        FROM ServiceProviderProfile sp
    //        LEFT JOIN GenderType g ON sp.GenderId = g.Id
    //        LEFT JOIN ParentCategory pc ON sp.ParentCategoryId = pc.Id
    //        WHERE sp.UserId = @UserId AND sp.IsDeleted = 0;

    //        SELECT st.Name 
    //        FROM ServiceProviderServiceTypes sst
    //        JOIN ServiceType st ON sst.ServiceTypeId = st.Id
    //        JOIN ServiceProviderProfile sp ON sst.ServiceProviderProfileId = sp.Id
    //        WHERE sp.UserId = @UserId;
    //        ";

    //    using var multi = await _dbConnection.QueryMultipleAsync(sql, new { UserId = userId });

    //    var profile = await multi.ReadFirstOrDefaultAsync<ServiceProviderProfile>();
    //    if (profile == null)
    //        return null;

    //    var serviceTypes = (await multi.ReadAsync<string>()).ToList();
    //    profile.ServiceTypes = serviceTypes;

    //    return profile;
    //}

    //public async Task<ServiceProviderProfile?> GetByUserIdAsync(int userId)
    //{
    //    var sql = @"SELECT * FROM ServiceProviderProfile WHERE UserId = @UserId AND IsDeleted = 0";
    //    return await _dbConnection.QueryFirstOrDefaultAsync<ServiceProviderProfile>(sql, new { UserId = userId });
    //}

    //public async Task<ServiceProviderProfile?> GetByUserIdAsync(int userId)
    //{
    //    var sql = @"
    //    SELECT 
    //        sp.*, 
    //        g.Name AS GenderName,
    //        pc.Name AS ParentCategoryName
    //    FROM ServiceProviderProfile sp
    //    LEFT JOIN GenderType g ON sp.GenderId = g.Id
    //    LEFT JOIN ParentCategory pc ON sp.ParentCategoryId = pc.Id
    //    WHERE sp.UserId = @UserId AND sp.IsDeleted = 0;

    //    SELECT st.Name 
    //    FROM ServiceProviderServiceTypes sst
    //    JOIN ServiceType st ON sst.ServiceTypeId = st.Id
    //    WHERE sst.ServiceProviderProfileId = (
    //        SELECT Id FROM ServiceProviderProfile WHERE UserId = @UserId AND IsDeleted = 0
    //    );
    //";

    //    using var multi = await _dbConnection.QueryMultipleAsync(sql, new { UserId = userId });

    //    var profile = await multi.ReadFirstOrDefaultAsync<ServiceProviderProfile>();
    //    if (profile == null)
    //        return null;

    //    var serviceTypes = (await multi.ReadAsync<string>()).ToList();
    //    profile.ServiceTypes = serviceTypes;

    //    return profile;
    //}


    public async Task<List<ServiceProviderProfile>> GetByParentCategoryIdAsync(int categoryId)
    {
        var sql = @"
        SELECT 
            spp.Id,
            spp.Name,
            spp.Surname,
            spp.City,
            spp.ExperienceYears,
            spp.Age,
            spp.AvatarUrl,
            spp.IsApprovedByAdmin AS IsApproved,
            g.Name AS GenderName,
            st.Name AS ServiceTypeName
        FROM ServiceProviderProfile spp
        LEFT JOIN GenderType g ON spp.GenderId = g.Id
        JOIN ServiceProviderServiceTypes sps ON sps.ServiceProviderProfileId = spp.Id
        JOIN ServiceType st ON st.Id = sps.ServiceTypeId
        WHERE spp.ParentCategoryId = @categoryId AND spp.IsDeleted = 0";

        var providerDict = new Dictionary<int, ServiceProviderProfile>();

        var result = await _dbConnection.QueryAsync<ServiceProviderProfile, string, ServiceProviderProfile>(
            sql,
            (provider, serviceType) =>
            {
                if (!providerDict.TryGetValue(provider.Id, out var currentProvider))
                {
                    currentProvider = provider;
                    currentProvider.ServiceTypes = new List<string>();
                    providerDict.Add(currentProvider.Id, currentProvider);
                }

                currentProvider.ServiceTypes.Add(serviceType);
                return currentProvider;
            },
            new { categoryId },
            splitOn: "ServiceTypeName"
        );

        return providerDict.Values.ToList();
    }


    public async Task<ServiceProviderProfile?> GetByUserIdAsync(int userId)
    {
        var sql = @"
        SELECT 
            sp.*, 
            g.Name AS GenderName,
            pc.Name AS ParentCategoryName
        FROM ServiceProviderProfile sp
        LEFT JOIN GenderType g ON sp.GenderId = g.Id
        LEFT JOIN ParentCategory pc ON sp.ParentCategoryId = pc.Id
        WHERE sp.UserId = @UserId AND sp.IsDeleted = 0;

        SELECT 
            st.Id, st.Name
        FROM ServiceProviderServiceTypes sst
        JOIN ServiceType st ON sst.ServiceTypeId = st.Id
        JOIN ServiceProviderProfile sp ON sst.ServiceProviderProfileId = sp.Id
        WHERE sp.UserId = @UserId AND sp.IsDeleted = 0;
    ";

        using var multi = await _dbConnection.QueryMultipleAsync(sql, new { UserId = userId });

        var profile = await multi.ReadFirstOrDefaultAsync<ServiceProviderProfile>();
        if (profile == null)
            return null;

        var serviceTypes = (await multi.ReadAsync<ServiceType>()).ToList();
        profile.ServiceTypes = serviceTypes.Select(s => s.Name).ToList();
        profile.ServiceTypeIds = serviceTypes.Select(s => s.Id).ToList();

        return profile;
    }

    public async Task<IEnumerable<dynamic>> GetTopRatedServiceProvidersRawAsync()
    {
        var sql = @"
            SELECT TOP 8
                spp.Id,
                spp.Name,
                spp.Surname,
                spp.City,
                spp.Age,
                spp.ExperienceYears,
                spp.AvatarUrl,
                ISNULL(AVG(CAST(r.Rating AS FLOAT)), 0) AS AverageRating,
                pc.Name AS ParentCategoryName,
                spp.CreatedAt
            FROM ServiceProviderProfile spp
            LEFT JOIN Review r ON spp.Id = r.ServiceProviderProfileId AND r.IsDeleted = 0
            LEFT JOIN ParentCategory pc ON spp.ParentCategoryId = pc.Id
            WHERE spp.IsDeleted = 0
            GROUP BY 
                spp.Id, spp.Name, spp.Surname, spp.City, spp.Age, spp.ExperienceYears, spp.AvatarUrl, pc.Name, spp.CreatedAt
            ORDER BY AverageRating DESC, spp.CreatedAt DESC
            ";

        var result = await _dbConnection.QueryAsync(sql);
        return result;
    }






}
