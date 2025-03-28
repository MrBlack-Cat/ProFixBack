using Application.CQRS.Notifications.Commands.Requests;
using Application.CQRS.Notifications.DTOs;
using AutoMapper;
using Common.GlobalResponse;
using Domain.Entities;
using MediatR;
using Repository.Common;
using Application.Common.Interfaces;

namespace Application.CQRS.Notifications.Commands.Handlers;

public class CreateNotificationCommandHandler : IRequestHandler<CreateNotificationCommand, ResponseModel<CreateNotificationDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IActivityLoggerService _activityLogger;

    public CreateNotificationCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IActivityLoggerService activityLogger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _activityLogger = activityLogger;
    }

    public async Task<ResponseModel<CreateNotificationDto>> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Dto;

        var entity = new Notification
        {
            UserId = dto.UserId,
            TypeId = dto.TypeId,
            Message = dto.Message,
            IsRead = dto.IsRead,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = request.CreatedByUserId
        };

        await _unitOfWork.NotificationRepository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();

        await _activityLogger.LogAsync(
            userId: request.CreatedByUserId,
            action: "Create",
            entityType: "Notification",
            entityId: entity.Id,
            performedBy: request.CreatedByUserId
        );

        var result = _mapper.Map<CreateNotificationDto>(entity);
        return new ResponseModel<CreateNotificationDto> { Data = result, IsSuccess = true };
    }
}
