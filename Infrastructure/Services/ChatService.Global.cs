using Common.DTOs;
using Common.DTOs.Common.DTOs;
using System.Text;
using System.Text.Json;

namespace Infrastructure.Services;

public partial class ChatService
{
    public async Task<string> GetGlobalBotResponseAsync(int userId, string userMessage)
    {
        await InitializeAsync();

        var serviceProviderProfile = await _unitOfWork.ServiceProviderProfileRepository.GetByUserIdAsync(userId);

        if (serviceProviderProfile == null)
            return "Ваш профиль сервис-провайдера не найден. Пожалуйста, создайте профиль, чтобы работать с данными.";

        var today = DateTime.UtcNow.Date;

        var globalData = new GlobalDataObject
        {
            Bookings = _bookings
                .Where(b => b.ServiceProviderProfileId == serviceProviderProfile.Id && !b.IsDeleted)
                .Select(b => new {
                    b.Id,
                    b.ScheduledDate,
                    StartTime = b.StartTime.ToString(@"hh\:mm"),
                    EndTime = b.EndTime.ToString(@"hh\:mm"),
                    b.Status
                }).ToList<object>(),

            Posts = _posts
                .Where(p => p.ServiceProviderProfileId == serviceProviderProfile.Id && !p.IsDeleted)
                .Select(p => new {
                    p.Id,
                    p.Title,
                    p.CreatedAt
                }).ToList<object>(),

            Certificates = _certificates
                .Where(c => c.ServiceProviderProfileId == serviceProviderProfile.Id && !c.IsDeleted)
                .Select(c => new {
                    c.Id,
                    c.Title,
                    IssuedAt = c.IssuedAt?.ToString("dd.MM.yyyy")
                }).ToList<object>(),

            Guarantees = _guarantees
                .Where(g => g.ServiceProviderProfileId == serviceProviderProfile.Id && !g.IsDeleted)
                .Select(g => new {
                    g.Id,
                    g.Title,
                    IssueDate = g.IssueDate?.ToString("dd.MM.yyyy"),
                    ExpirationDate = g.ExpirationDate?.ToString("dd.MM.yyyy")
                }).ToList<object>()
        };

        var globalJson = JsonSerializer.Serialize(globalData, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        var prompt = $@"
                Ты — профессиональный помощник платформы ProFix.

                Сегодняшняя дата: {today:dd.MM.yyyy}.

                Вот таблица всех данных для сервис-провайдера {serviceProviderProfile.Name} {serviceProviderProfile.Surname}:
                {globalJson}

                Вопрос пользователя:
                ""{userMessage}""

                Твоя задача:
                - с начало анализируй полнестю всю таблицы, а затем отвечай на вопрос пользователя.    
                - Используй ТОЛЬКО данные из таблицы.
                - Разделяй логически ответы: бронирования, посты, сертификаты, гарантии.
                - Отвечай детально, чётко, вежливо и на русском языке.
                - Если какой-то информации нет — честно сообщи.
                - Ничего не выдумывай.
                ";

        return await SendPromptToBotAsync(prompt);
    }
}
