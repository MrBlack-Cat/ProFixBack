using System.Text;
using System.Text.Json;

namespace Infrastructure.Services;

public partial class ChatService
{
    private async Task<string> SendPromptToBotAsync(string prompt)
    {
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
            Console.WriteLine($"❌ Ошибка при отправке промпта: {ex.Message}");
            return "Извините, произошла ошибка при обработке вашего запроса.";
        }
    }
}
