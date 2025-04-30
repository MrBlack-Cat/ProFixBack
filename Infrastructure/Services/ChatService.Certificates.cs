using System.Text;
using System.Text.Json;

namespace Infrastructure.Services;

public partial class ChatService
{
    public async Task<string> GetCertificatesBotResponseAsync(int userId, string userMessage)
    {
        await InitializeAsync();

        var serviceProviderProfile = await _unitOfWork.ServiceProviderProfileRepository.GetByUserIdAsync(userId);

        if (serviceProviderProfile == null)
            return "Ваш профиль сервис-провайдера не найден. Пожалуйста, создайте профиль для работы с сертификатами.";

        var certificatesForProvider = _certificates
            .Where(c => c.ServiceProviderProfileId == serviceProviderProfile.Id && !c.IsDeleted)
            .OrderByDescending(c => c.CreatedAt)
            .ToList();

        var detailedCertificates = certificatesForProvider.Select(c => new
        {
            CertificateId = c.Id,
            Title = c.Title,
            Description = c.Description ?? "Нет описания",
            IssuedAt = c.IssuedAt?.ToString("dd.MM.yyyy") ?? "Дата выпуска неизвестна"
        }).ToList();

        var certificatesJson = JsonSerializer.Serialize(detailedCertificates, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        var today = DateTime.UtcNow.Date;

        var prompt = $@"
                    Ты — профессиональный помощник платформы ProFix.

                    Сегодняшняя дата: {today:dd.MM.yyyy}.

                    Вот таблица сертификатов сервис-провайдера {serviceProviderProfile.Name} {serviceProviderProfile.Surname}:
                    {certificatesJson}

                    Вопрос пользователя:
                    ""{userMessage}""

                    Твоя задача:
                    - с начало анализируй полнестю всю таблицу сертификатов, а затем отвечай на вопрос пользователя.    
                    - Используй **ТОЛЬКО** данные из таблицы сертификатов.
                    - Пиши детально, чётко, грамотно и строго на русском языке.
                    - Анализируй:
                      - Название сертификата (Title).
                      - Дату выдачи (IssuedAt).
                      - Наличие описания (Description).
                    - Если спрашивают:
                      - «сколько сертификатов» — посчитай количество.
                      - «когда был выдан сертификат» — укажи дату IssuedAt.
                      - «есть ли описание» — посмотри в поле Description.
                    - Ничего не выдумывай и не додумывай.
                    - Если сертификатов нет — честно сообщи об этом.
                    - Не путай сертификаты с бронированиями, постами и гарантийными документами.
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

            return botMessage ?? "Извините, я не смог обработать ваш запрос.";
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Ошибка API: {ex.Message}");
            return "Извините, произошла ошибка на сервере.";
        }
    }
}
