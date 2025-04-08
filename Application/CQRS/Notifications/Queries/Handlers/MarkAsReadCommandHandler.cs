using Application.CQRS.Notifications.Commands.Requests;
using Application.Common.Interfaces;
using Common.Exceptions;
using Common.GlobalResponse;
using MediatR;
using Repository.Common;

namespace Application.CQRS.Notifications.Commands.Handlers;

public class MarkAsReadCommandHandler : IRequestHandler<MarkAsReadCommand, ResponseModel<string>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;
    private readonly IActivityLoggerService _activityLogger;

    public MarkAsReadCommandHandler(IUnitOfWork unitOfWork, IUserContext userContext, IActivityLoggerService activityLogger)
    {
        _unitOfWork = unitOfWork;
        _userContext = userContext;
        _activityLogger = activityLogger;
    }

    public async Task<ResponseModel<string>> Handle(MarkAsReadCommand request, CancellationToken cancellationToken)
    {
        var notification = await _unitOfWork.NotificationRepository.GetByIdAsync(request.NotificationId);
        if (notification == null || notification.IsDeleted)
            throw new NotFoundException("Notification not found.");

        if (notification.UserId != request.UpdatedByUserId)
            throw new ForbiddenException("You are not allowed to mark this notification.");

        notification.IsRead = true;
        notification.UpdatedAt = DateTime.UtcNow;
        notification.UpdatedBy = request.UpdatedByUserId;

        await _unitOfWork.NotificationRepository.UpdateAsync(notification);
        await _unitOfWork.SaveChangesAsync();

        await _activityLogger.LogAsync(
            userId: request.UpdatedByUserId,
            action: "MarkAsRead",
            entityType: "Notification",
            entityId: notification.Id,
            performedBy: request.UpdatedByUserId
        );

        return new ResponseModel<string>
        {
            Data = "Notification marked as read.",
            IsSuccess = true
        };
    }
}
