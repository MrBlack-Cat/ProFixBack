using Application.CQRS.Notifications.DTOs;
using Common.GlobalResponse;
using MediatR;

namespace Application.CQRS.Notifications.Commands.Requests;

public record CreateNotificationCommand(CreateNotificationDto Dto, int CreatedByUserId)
    : IRequest<ResponseModel<CreateNotificationDto>>;
