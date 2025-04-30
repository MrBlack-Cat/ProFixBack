using Domain.Entities;
using Repository.Common;

namespace Repository.Repositories;

public interface IMessageRepository : IRepository<Message>
{
    Task<IEnumerable<Message>> GetByUserIdAsync(int userId);
    Task<IEnumerable<Message>> GetAllBetweenUsersAsync(int userId1, int userId2);
    Task<IEnumerable<dynamic>> GetRawChatSummariesByUserIdAsync(int userId);
    Task<int> GetUnreadCountByUserIdAsync(int userId);
    Task<IEnumerable<Message>> GetAllMessagesAsync();


}
