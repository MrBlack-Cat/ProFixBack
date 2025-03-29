using Common.GlobalResponse;
using MediatR;

namespace Application.CQRS.Notifications.Commands.Requests;

public record DeleteNotificationCommand(int Id, int DeletedByUserId, string Reason)
    : IRequest<ResponseModel<string>>;
