using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public interface IChatMessageRepository
    {
        Task<IEnumerable<ChatMessage>> GetMessagesByUserIdAsync(int userId);
        Task AddMessageAsync(ChatMessage message);
    }
    
}
