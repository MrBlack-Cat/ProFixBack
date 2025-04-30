using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using Common.Interfaces;
using Repository.Repositories;
using Domain.Entities;

namespace WebApi.Hubs
{
    public class ChatHub : Hub
    {
        private static readonly ConcurrentDictionary<string, string> _connections = new();
        private readonly IChatService _chatService;
        private readonly IChatMessageRepository _chatMessageRepository;

        public ChatHub(IChatService chatService, IChatMessageRepository chatMessageRepository)
        {
            _chatService = chatService;
            _chatMessageRepository = chatMessageRepository;
        }

        public override async Task OnConnectedAsync()
        {
            var sessionId = Context.GetHttpContext()?.Request.Query["sessionId"].ToString();
            var userId = Context.GetHttpContext()?.Request.Query["userId"].ToString();

            var identifier = !string.IsNullOrEmpty(userId) ? userId : sessionId;

            if (!string.IsNullOrEmpty(identifier))
            {
                _connections[identifier] = Context.ConnectionId;
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var connection = _connections.FirstOrDefault(x => x.Value == Context.ConnectionId);
            if (!string.IsNullOrEmpty(connection.Key))
            {
                _connections.TryRemove(connection.Key, out _);
            }

            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string userId, string message)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(message))
                return;

            if (_connections.TryGetValue(userId, out var connectionId))
            {
                if (!int.TryParse(userId, out var userIdInt))
                {
                    await Clients.Client(connectionId).SendAsync("ReceiveMessage", "Bot", "Ошибка идентификатора пользователя.");
                    return;
                }

                // save message
                await _chatMessageRepository.AddMessageAsync(new ChatMessage
                {
                    UserId = userIdInt,
                    Sender = "User",
                    MessageText = message,
                    CreatedAt = DateTime.UtcNow
                });

                //botun cavabi
                var botResponse = await _chatService.GetBotResponseAsync(userIdInt, message);

                // save bot response
                await _chatMessageRepository.AddMessageAsync(new ChatMessage
                {
                    UserId = userIdInt,
                    Sender = "Bot",
                    MessageText = botResponse,
                    CreatedAt = DateTime.UtcNow
                });

                // send message to client
                await Clients.Client(connectionId).SendAsync("ReceiveMessage", "Bot", botResponse);
            }
            else
            {
                Console.WriteLine($"❌ Нет соединения для userId {userId}");
            }
        }

    }
}
