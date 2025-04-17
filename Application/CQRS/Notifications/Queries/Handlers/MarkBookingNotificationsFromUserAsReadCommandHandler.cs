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
    public class MarkBookingNotificationsFromUserAsReadCommandHandler
        : IRequestHandler<MarkBookingNotificationsFromUserAsReadCommand, ResponseModel<string>>
    {
        private readonly INotificationRepository _notificationRepository;

        public MarkBookingNotificationsFromUserAsReadCommandHandler(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<ResponseModel<string>> Handle(MarkBookingNotificationsFromUserAsReadCommand request, CancellationToken cancellationToken)
        {
            await _notificationRepository.MarkAllBookingNotificationsFromUserAsync(request.CurrentUserId, request.SenderUserId);
            return ResponseModel<string>.Success("Booking notifications marked as read.");
        }
    }

}
