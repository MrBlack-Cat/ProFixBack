using Common.GlobalResponse;
using MediatR;
using Application.CQRS.Notifications.DTOs;

namespace Application.CQRS.Notifications.Queries.Requests;

public record GetNotificationByIdQuery(int Id) : IRequest<ResponseModel<GetNotificationByIdDto>>;
