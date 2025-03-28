using Application.CQRS.Notifications.Commands.Requests;
using Common.Exceptions;
using Common.GlobalResponse;
using MediatR;
using Repository.Common;
using Application.Common.Interfaces;

namespace Application.CQRS.Notifications.Commands.Handlers;

public class DeleteNotificationCommandHandler : IRequestHandler<DeleteNotificationCommand, ResponseModel<string>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthorizationService _authorizationService;
    private readonly IUserContext _userContext;
    private readonly IActivityLoggerService _activityLogger;

    public DeleteNotificationCommandHandler(
        IUnitOfWork unitOfWork,
        IAuthorizationService authorizationService,
        IUserContext userContext,
        IActivityLoggerService activityLogger)
    {
        _unitOfWork = unitOfWork;
        _authorizationService = authorizationService;
        _userContext = userContext;
        _activityLogger = activityLogger;
    }

    public async Task<ResponseModel<string>> Handle(DeleteNotificationCommand request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.NotificationRepository.GetByIdAsync(request.Id);
        if (entity is null || entity.IsDeleted)
            throw new NotFoundException("Notification not found.");

        if (entity.CreatedBy == null)
            throw new ForbiddenException("Unable to verify owner of this notification.");

        _authorizationService.AuthorizeOwnerOrAdmin(
            resourceOwnerId: entity.CreatedBy.Value,
            currentUserId: request.DeletedByUserId,
            currentUserRole: _userContext.GetUserRole()
        );

        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        entity.DeletedBy = request.DeletedByUserId;
        entity.DeletedReason = request.Reason;

        await _unitOfWork.NotificationRepository.UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync();

        await _activityLogger.LogAsync(
            userId: entity.CreatedBy ?? request.DeletedByUserId,
            action: "Delete",
            entityType: "Notification",
            entityId: entity.Id,
            performedBy: request.DeletedByUserId,
            description: request.Reason
        );

        return new ResponseModel<string> { Data = "Notification deleted successfully.", IsSuccess = true };
    }
}
