using Application.Common.Interfaces;
using Application.CQRS.Notifications.DTOs;
using Domain.Entities;
using Repository.Repositories;

namespace Infrastructure.Services;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _notificationRepository;

    public NotificationService(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    // Message
    public async Task CreateAsync(int receiverUserId, int typeId, string message, int createdBy)
    {
        var notification = new Notification
        {
            UserId = receiverUserId,
            TypeId = typeId,
            Message = message,
            IsRead = false,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = createdBy
        };

        await _notificationRepository.AddAsync(notification);
    }

    // booking
    public async Task CreateAsync(CreateNotificationDto dto)
    {
        var notification = new Notification
        {
            UserId = dto.UserId,
            TypeId = dto.TypeId,
            Message = dto.Message,
            IsRead = false,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = dto.CreatedBy
        };

        await _notificationRepository.AddAsync(notification);
    }
}
