using Application.CQRS.Notifications.DTOs;
using Common.GlobalResponse;
using MediatR;

namespace Application.CQRS.Notifications.Commands.Requests;

public record UpdateNotificationCommand(int Id, int UpdatedByUserId, UpdateNotificationDto Dto)
    : IRequest<ResponseModel<UpdateNotificationDto>>;
