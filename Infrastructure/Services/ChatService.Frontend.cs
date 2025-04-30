using System.Text;
using System.Text.Json;

namespace Infrastructure.Services;

public partial class ChatService
{
    public async Task<string> GetFrontendHelpResponseAsync(string userMessage)
    {
        var today = DateTime.UtcNow.Date;

        var prompt = $@"
                Ты — помощник платформы ProFix, обученный помогать пользователям с действиями на сайте.

                Сегодня: {today:dd.MM.yyyy}

                Вот запрос пользователя:
                ""{ userMessage}""

                Твоя задача:
                -Объясни пошагово, как выполнить действие на сайте(войти в профиль, найти вкладку, добавить пост и т.д.).
                - Упоминай названия кнопок или вкладок, если это уместно(например: «нажмите на иконку профиля в правом верхнем углу»).
                -Пиши кратко, понятно и на русском языке.
                - Отвечай вежливо и дружелюбно.
                ";

        var requestBody = new
        {
            model = "openai/gpt-3.5-turbo",
            messages = new[]
            {
                new { role = "system", content = "Ты — профессиональный помощник ProFix, обученный помогать с действиями на сайте." },
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

            return json.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString() ?? "Извините, я не смог обработать ваш вопрос.";
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Ошибка API: {ex.Message}");
            return "Извините, произошла ошибка на сервере.";
        }
    }
}
