using Application.CQRS.Notifications.Queries.Requests;
using Common.GlobalResponse;
using MediatR;
using Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Notifications.Queries.Handlers
{
    public class MarkNotificationsFromUserAsReadCommandHandler : IRequestHandler<MarkNotificationsFromUserAsReadCommand, ResponseModel<string>>
    {
        private readonly INotificationRepository _notificationRepository;

        public MarkNotificationsFromUserAsReadCommandHandler(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<ResponseModel<string>> Handle(MarkNotificationsFromUserAsReadCommand request, CancellationToken cancellationToken)
        {
            await _notificationRepository.MarkAllFromUserAsReadAsync(request.CurrentUserId, request.SenderUserId);
            return ResponseModel<string>.Success("Notifications marked as read.");
        }
    }

}
