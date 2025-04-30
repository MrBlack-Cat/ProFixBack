using System.Text;
using System.Text.Json;

namespace Infrastructure.Services;

public partial class ChatService
{
    public async Task<string> GetGuaranteesBotResponseAsync(int userId, string userMessage)
    {
        await InitializeAsync();

        var serviceProviderProfile = await _unitOfWork.ServiceProviderProfileRepository.GetByUserIdAsync(userId);

        if (serviceProviderProfile == null)
            return "Ваш профиль сервис-провайдера не найден. Пожалуйста, создайте профиль, чтобы работать с гарантийными документами.";

        var guaranteesForProvider = _guarantees
            .Where(g => g.ServiceProviderProfileId == serviceProviderProfile.Id && !g.IsDeleted)
            .OrderByDescending(g => g.CreatedAt)
            .ToList();

        var detailedGuarantees = guaranteesForProvider.Select(g => new
        {
            GuaranteeId = g.Id,
            Title = g.Title,
            IssuedAt = g.IssueDate?.ToString("dd.MM.yyyy") ?? "Не указана дата выдачи",
            ExpirationAt = g.ExpirationDate?.ToString("dd.MM.yyyy") ?? "Не указана дата окончания",
            ClientProfileId = g.ClientProfileId
        }).ToList();

        var guaranteesJson = JsonSerializer.Serialize(detailedGuarantees, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        var today = DateTime.UtcNow.Date;

        var prompt = $@"
                Ты — профессиональный помощник платформы ProFix.

                Сегодняшняя дата: {today:dd.MM.yyyy}.

                Вот полная таблица гарантийных документов сервис-провайдера {serviceProviderProfile.Name} {serviceProviderProfile.Surname}:
                {guaranteesJson}

                Вопрос пользователя:
                ""{userMessage}""

                Твоя задача:
                - с начало анализируй полнестю всю таблицу Гарантий, а затем отвечай на вопрос пользователя.    
                - Используй **ТОЛЬКО** предоставленные данные о гарантиях.
                - Пиши ответ строго на русском языке, детально, профессионально и дружелюбно.
                - Учитывай:
                  - Дату выдачи (IssuedAt).
                  - Дату окончания гарантии (ExpirationAt).
                  - Количество гарантийных документов.
                  - Привязку к ClientProfileId (если спрашивают, кому принадлежит гарантия).
                - Если пользователь спрашивает:
                  - «сколько гарантий» — подсчитай количество записей.
                  - «действующие гарантии» — найди те, у которых дата окончания позже сегодняшней.
                  - «когда заканчивается гарантия» — выведи дату окончания.
                - Не путай гарантии с постами, сертификатами или бронированиями.
                - Если гарантий нет — честно сообщи об этом.
                - Никакую информацию не выдумывай.
                ";

        var requestBody = new
        {
            model = "openai/gpt-3.5-turbo",
            messages = new[]
            {
                new { role = "system", content = "Ты — профессиональный помощник платформы ProFix." },
                new { role = "user", content = prompt }
            }
        };

        var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

        try
        {
            var response = await _httpClient.PostAsync("https://openrouter.ai/api/v1/chat/completions", content);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            var json = JsonDocument.Parse(responseBody);

            var botMessage = json.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            return botMessage ?? "Извините, я не смог понять ваш запрос.";
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Ошибка API: {ex.Message}");
            return "Извините, произошла ошибка на сервере.";
        }
    }
}
