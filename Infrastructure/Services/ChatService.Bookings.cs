using System.Text;
using System.Text.Json;

namespace Infrastructure.Services;

public partial class ChatService
{
    public async Task<string> GetBookingBotResponseAsync(int userId, string userMessage)
    {
        await InitializeAsync();

        var serviceProviderProfile = await _unitOfWork.ServiceProviderProfileRepository.GetByUserIdAsync(userId);

        if (serviceProviderProfile == null)
            return "Ваш профиль сервис-провайдера не найден. Пожалуйста, создайте профиль для управления бронированиями.";

        var bookingsForProvider = _bookings
            .Where(b => b.ServiceProviderProfileId == serviceProviderProfile.Id && !b.IsDeleted)
            .ToList();

        var detailedBookings = (from booking in bookingsForProvider
                                join client in _clients on booking.ClientProfileId equals client.Id
                                select new
                                {
                                    BookingId = booking.Id,
                                    ScheduledDate = booking.ScheduledDate,
                                    StartTime = booking.StartTime != TimeSpan.Zero ? booking.StartTime.ToString(@"hh\:mm") : "00:00",
                                    EndTime = booking.EndTime != TimeSpan.Zero ? booking.EndTime.ToString(@"hh\:mm") : "00:00",
                                    ClientName = $"{client.Name} {client.Surname}",
                                    ClientCity = client.City,
                                    Status = booking.Status ?? "Неизвестно",
                                    IsConfirmedByProvider = booking.IsConfirmedByProvider,
                                    IsCompleted = booking.IsCompleted
                                }).ToList();

        var bookingsJson = JsonSerializer.Serialize(detailedBookings, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        var today = DateTime.UtcNow.Date;

        var prompt = $@"
                Ты — дружелюбный, точный и профессиональный помощник платформы ProFix.

                Сегодня: {today:dd.MM.yyyy}.

                Перед тобой полная таблица всех бронирований сервис-провайдера {serviceProviderProfile.Name} {serviceProviderProfile.Surname}:
                {bookingsJson}

                Вопрос пользователя:
                ""{userMessage}""

                Твоя задача:
                - с начало анализируй полнестю всю таблицу бронирований, а затем отвечай на вопрос пользователя.    
                - Используй **ТОЛЬКО** предоставленные данные из таблицы.
                - Отвечай строго на русском языке, вежливо и профессионально.
                - Учитывай следующие параметры бронирования: дата (ScheduledDate), время начала и окончания (StartTime, EndTime), статус (Status), подтверждение (IsConfirmedByProvider), завершение (IsCompleted).
                - Если пользователь спрашивает про «сегодня», «вчера», «завтра» — соотнеси это с датой {today:dd.MM.yyyy}.
                - Если бронирования отсутствуют на указанную дату — честно сообщи об этом.
                - Если вопрос о ближайшем бронировании — найди ближайшее будущее бронирование по дате.
                - Не путай бронирования с постами, сертификатами, гарантиями и другими сущностями.
                - Не выдумывай информацию, которой нет в таблице.
                - Отвечай детально, логично и по существу, избегая лишних слов.
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
