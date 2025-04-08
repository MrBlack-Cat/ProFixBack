using Common.GlobalResponse;
using MediatR;

namespace Application.CQRS.Notifications.Commands.Requests;

public record struct MarkAsReadCommand(int NotificationId, int UpdatedByUserId)
    : IRequest<ResponseModel<string>>;
