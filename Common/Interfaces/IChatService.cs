using Common.DTOs;

namespace Common.Interfaces
{
    public interface IChatService
    {
        Task<string> GetBotResponseAsync(int userId, string userMessage, string? sessionId = null);
        Task<IEnumerable<ChatMessageDto>> GetMessagesByUserIdAsync(int userId);
        Task InitializeAsync();
    }
}
