using Common.GlobalResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Notifications.Queries.Requests
{
    public record MarkNotificationsFromUserAsReadCommand(int CurrentUserId, int SenderUserId)
        : IRequest<ResponseModel<string>>;

}
