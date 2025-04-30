using Domain.Entities;
using System.Text;
using System.Text.Json;

namespace Infrastructure.Services;

public partial class ChatService
{
    public async Task<string> GetPostsBotResponseAsync(int userId, string userMessage)
    {
        await InitializeAsync();

        var serviceProviderProfile = await _unitOfWork.ServiceProviderProfileRepository.GetByUserIdAsync(userId);

        if (serviceProviderProfile == null)
            return "Ваш профиль сервис-провайдера не найден. Пожалуйста, создайте профиль для публикации постов.";

        var postsForProvider = _posts
            .Where(p => p.ServiceProviderProfileId == serviceProviderProfile.Id && !p.IsDeleted)
            .ToList();

        var detailedPosts = (from post in postsForProvider
                             let likesCount = _postLikes.Count(l => l.PostId == post.Id)
                             select new
                             {
                                 PostId = post.Id,
                                 Title = post.Title,
                                 Content = post.Content,
                                 CreatedAt = post.CreatedAt,
                                 Likes = likesCount
                             }).OrderByDescending(p => p.CreatedAt).ToList();

        var postsJson = JsonSerializer.Serialize(detailedPosts, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        var today = DateTime.UtcNow.Date;

        var prompt = $@"
                Ты — дружелюбный, точный и профессиональный помощник платформы ProFix.

                Сегодня: {today:dd.MM.yyyy}.

                Перед тобой полная таблица всех постов сервис-провайдера {serviceProviderProfile.Name} {serviceProviderProfile.Surname}:
                {postsJson}

                Вопрос пользователя:
                ""{userMessage}""

                Твоя задача:
                - с начало анализируй полнестю всю таблицу бронирований, а затем отвечай на вопрос пользователя.    
                - Используй **ТОЛЬКО** данные из таблицы.
                - Отвечай строго на русском языке, детально, вежливо и профессионально.
                - Учитывай следующие параметры: дата создания поста (CreatedAt), заголовок (Title), текст поста (Content), количество лайков (Likes).
                - Если пользователь спрашивает:
                  - «сколько постов» — подсчитай общее количество.
                  - «последний пост» — найди самый новый пост.
                  - «самый популярный пост» — найди пост с максимальным количеством лайков.
                  - «когда был последний пост» — укажи дату создания последнего поста.
                  - «сколько лайков» — подсчитай общее количество лайков по всем постам.
                - Не путай посты с бронированиями, сертификатами и другими данными.
                - Если постов нет — сообщи об этом честно.
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
