using Application.CQRS.Notifications.DTOs;
using Common.GlobalResponse;
using MediatR;

namespace Application.CQRS.Notifications.Queries.Requests;

public record GetUnreadNotificationsQuery(int UserId) : IRequest<ResponseModel<List<NotificationListDto>>>;
