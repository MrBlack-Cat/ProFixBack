using Application.Common.Interfaces;
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
}
