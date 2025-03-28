using Application.CQRS.Notifications.Commands.Requests;
using Application.CQRS.Notifications.DTOs;
using Application.Common.Interfaces;
using AutoMapper;
using Common.Exceptions;
using Common.GlobalResponse;
using MediatR;
using Repository.Common;

namespace Application.CQRS.Notifications.Commands.Handlers;

public class UpdateNotificationCommandHandler : IRequestHandler<UpdateNotificationCommand, ResponseModel<UpdateNotificationDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IAuthorizationService _authorizationService;
    private readonly IUserContext _userContext;
    private readonly IActivityLoggerService _activityLogger;

    public UpdateNotificationCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IAuthorizationService authorizationService,
        IUserContext userContext,
        IActivityLoggerService activityLogger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _authorizationService = authorizationService;
        _userContext = userContext;
        _activityLogger = activityLogger;
    }

    public async Task<ResponseModel<UpdateNotificationDto>> Handle(UpdateNotificationCommand request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.NotificationRepository.GetByIdAsync(request.Id);
        if (entity is null || entity.IsDeleted)
            throw new NotFoundException("Notification not found.");

        if (entity.CreatedBy == null)
            throw new ForbiddenException("Unable to verify owner of this notification.");

        _authorizationService.AuthorizeOwnerOrAdmin(
            resourceOwnerId: entity.CreatedBy.Value,
            currentUserId: request.UpdatedByUserId,
            currentUserRole: _userContext.GetUserRole()
        );

        entity.TypeId = request.Dto.TypeId;
        entity.Message = request.Dto.Message;
        entity.IsRead = request.Dto.IsRead;
        entity.UpdatedAt = DateTime.UtcNow;
        entity.UpdatedBy = request.UpdatedByUserId;

        await _unitOfWork.NotificationRepository.UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync();

        await _activityLogger.LogAsync(
            userId: entity.CreatedBy ?? request.UpdatedByUserId,
            action: "Update",
            entityType: "Notification",
            entityId: entity.Id,
            performedBy: request.UpdatedByUserId
        );

        var dto = _mapper.Map<UpdateNotificationDto>(entity);
        return new ResponseModel<UpdateNotificationDto> { Data = dto, IsSuccess = true };
    }
}
