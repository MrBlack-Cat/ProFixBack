using Domain.Entities;
using Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories;

public interface INotificationRepository : IRepository<Notification>
{
    Task<IEnumerable<Notification>> GetByUserIdAsync(int userId);
    Task<List<Notification>> GetUnreadByUserIdAsync(int userId);
    Task MarkAllFromUserAsReadAsync(int currentUserId, int senderUserId);
    Task MarkAllBookingNotificationsFromUserAsync(int currentUserId, int senderUserId);


}
