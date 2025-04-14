using Dapper;
using Domain.Entities;
using Domain.Enums;
using Repository.Repositories;
using System.Data;
using System.Security.Claims;

namespace Dal.SqlServer.Infrastructure;

public class SqlServiceBookingRepository : IServiceBookingRepository
{
    private readonly IDbConnection _db;

    public SqlServiceBookingRepository(IDbConnection db)
    {
        _db = db;
    }

    public async Task AddAsync(ServiceBooking entity)
    {
        entity.StatusId = (int)ServiceBookingStatusEnum.Pending;
        entity.Status = "Pending";

        var sql = @"
        INSERT INTO ServiceBooking
        (ClientProfileId, ServiceProviderProfileId, Description, StatusId, Status, ScheduledDate,
         StartTime, EndTime, IsConfirmedByProvider, ConfirmationDate, IsCompleted, CompletionDate,
         CreatedAt, CreatedBy, IsDeleted)
        VALUES
        (@ClientProfileId, @ServiceProviderProfileId, @Description, @StatusId, @Status, @ScheduledDate,
         @StartTime, @EndTime, @IsConfirmedByProvider, @ConfirmationDate, @IsCompleted, @CompletionDate,
         @CreatedAt, @CreatedBy, 0);
        SELECT CAST(SCOPE_IDENTITY() as int);";

        var id = await _db.ExecuteScalarAsync<int>(sql, entity);
        entity.Id = id;
    }



    public async Task UpdateAsync(ServiceBooking entity)
    {
        var sql = @"
            UPDATE ServiceBooking SET
                Description = @Description,
                StatusId = @StatusId,
                Status = @Status,
                ScheduledDate = @ScheduledDate,
                StartTime = @StartTime,
                EndTime = @EndTime,
                IsConfirmedByProvider = @IsConfirmedByProvider,
                ConfirmationDate = @ConfirmationDate,
                IsCompleted = @IsCompleted,
                CompletionDate = @CompletionDate,
                PendingDate = @PendingDate,
                RejectedDate = @RejectedDate,
                CancelledDate = @CancelledDate,
                InProgressDate = @InProgressDate,
                UpdatedAt = @UpdatedAt,
                UpdatedBy = @UpdatedBy,
                IsDeleted = @IsDeleted,
                DeletedAt = @DeletedAt,
                DeletedBy = @DeletedBy,
                DeletedReason = @DeletedReason
            WHERE Id = @Id";

        await _db.ExecuteAsync(sql, entity);
    }

    public async Task DeleteAsync(int id, ClaimsPrincipal user)
    {
        var userId = int.Parse(user.FindFirst("nameid")?.Value ?? "0");

        var entity = await GetByIdAsync(id);
        if (entity is null)
            return;

        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        entity.DeletedBy = userId;

        var sql = @"
        UPDATE ServiceBooking
        SET IsDeleted = 1,
            DeletedAt = @DeletedAt,
            DeletedBy = @DeletedBy
        WHERE Id = @Id";

        await _db.ExecuteAsync(sql, new
        {
            entity.DeletedAt,
            entity.DeletedBy,
            entity.Id
        });
    }

    public async Task DeleteAsync(ServiceBooking entity)
    {
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;

        var sql = @"
        UPDATE ServiceBooking
        SET IsDeleted = 1,
            DeletedAt = @DeletedAt
        WHERE Id = @Id";

        await _db.ExecuteAsync(sql, new
        {
            entity.DeletedAt,
            entity.Id
        });
    }


    public async Task<ServiceBooking?> GetByIdAsync(int id)
    {
        var sql = @"SELECT * FROM ServiceBooking WHERE Id = @id";
        return await _db.QueryFirstOrDefaultAsync<ServiceBooking>(sql, new { id });
    }
    public async Task<IEnumerable<ServiceBooking>> GetAllAsync()
    {
        var sql = @"SELECT * FROM ServiceBooking WHERE IsDeleted = 0 ORDER BY ScheduledDate DESC, StartTime";
        var result = await _db.QueryAsync<ServiceBooking>(sql);
        return result;
    }


    public async Task<List<ServiceBooking>> GetByClientProfileIdAsync(int clientProfileId)
    {
        var sql = @"
                SELECT 
                    b.Id,
                    b.Description,
                    b.ScheduledDate,
                    b.StartTime,
                    b.EndTime,
                    b.IsConfirmedByProvider,
                    b.ConfirmationDate,
                    b.IsCompleted,
                    b.CompletionDate,
                    b.StatusId,
                    b.PendingDate,
                    b.RejectedDate,
                    b.CancelledDate,
                    b.InProgressDate,
                    b.CreatedAt,
                    s.Name AS Status,
                    sp.Id AS ServiceProviderProfileId,
                    sp.Name + ' ' + sp.Surname AS ServiceProviderName,
                    sp.AvatarUrl AS ServiceProviderAvatarUrl
                FROM ServiceBooking b
                LEFT JOIN ServiceBookingStatus s ON b.StatusId = s.Id
                LEFT JOIN ServiceProviderProfile sp ON b.ServiceProviderProfileId = sp.Id
                WHERE b.ClientProfileId = @clientProfileId AND b.IsDeleted = 0
                ORDER BY b.ScheduledDate DESC, b.StartTime";
        var result = await _db.QueryAsync<ServiceBooking>(sql, new { clientProfileId });
        return result.ToList();
    }

    public async Task<List<ServiceBooking>> GetByProviderProfileIdAsync(int providerProfileId)
    {
        var sql = @"
        SELECT 
            b.Id,
            b.Description,
            b.ScheduledDate,
            b.StartTime,
            b.EndTime,
            b.IsConfirmedByProvider,
            b.ConfirmationDate,
            b.IsCompleted,
            b.CompletionDate,
            b.StatusId,
            b.PendingDate,
            b.RejectedDate,
            b.CancelledDate,
            b.InProgressDate,
            b.CreatedAt,
            s.Name AS Status,

            cp.Name + ' ' + cp.Surname AS ClientName,
            cp.AvatarUrl AS ClientAvatarUrl,
            cp.Id AS ClientProfileId

        FROM ServiceBooking b
        LEFT JOIN ServiceBookingStatus s ON b.StatusId = s.Id
        LEFT JOIN ClientProfile cp ON b.ClientProfileId = cp.Id
        WHERE b.ServiceProviderProfileId = @providerProfileId AND b.IsDeleted = 0
        ORDER BY b.ScheduledDate DESC, b.StartTime";

        var result = await _db.QueryAsync<ServiceBooking>(sql, new { providerProfileId });
        return result.ToList();
    }



    public async Task<ServiceBooking?> GetDetailedByIdAsync(int id)
    {
        var sql = @"
            SELECT b.*, s.Name AS StatusName
            FROM ServiceBooking b
            LEFT JOIN ServiceBookingStatus s ON b.StatusId = s.Id
            WHERE b.Id = @id AND b.IsDeleted = 0";

        // здесь ты можешь использовать multi-mapping, если нужно включить объекты навигации
        var result = await _db.QueryFirstOrDefaultAsync<ServiceBooking>(sql, new { id });
        return result;
    }

    public async Task<bool> IsTimeSlotAvailableAsync(int providerId, DateTime date, TimeSpan start, TimeSpan end)
    {
        var sql = @"
        SELECT COUNT(1)
        FROM ServiceBooking
        WHERE ServiceProviderProfileId = @providerId
          AND CAST(ScheduledDate AS DATE) = @date
          AND IsDeleted = 0
          AND (
              (@start < EndTime AND @end > StartTime)
          )";

        var count = await _db.ExecuteScalarAsync<int>(sql, new { providerId, date, start, end });
        return count == 0;
    }

    public async Task<List<ServiceBooking>> GetByDateAsync(int providerId, DateTime date)
    {
        var sql = @"
        SELECT StartTime, EndTime 
        FROM ServiceBooking 
        WHERE ServiceProviderProfileId = @providerId 
          AND ScheduledDate = @date 
          AND IsDeleted = 0";

        var result = await _db.QueryAsync<ServiceBooking>(sql, new { providerId, date });
        return result.ToList();
    }




}
