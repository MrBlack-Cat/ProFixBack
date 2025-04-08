using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ProFix.IntegrationTests.Helpers;

public static class IntegrationTestHelper
{
    // Метод с указанием email и password
    public static async Task<string> GetJwtTokenAsync(HttpClient client, string email, string password)
    {
        var credentials = new
        {
            email,
            password
        };

        var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(credentials), Encoding.UTF8, "application/json");
        var response = await client.PostAsync("/api/Auth/Login", content);

        response.EnsureSuccessStatusCode();

        var responseBody = await response.Content.ReadAsStringAsync();
        var json = JObject.Parse(responseBody);

        return json["data"]?["token"]?.ToString() ?? throw new Exception("Token not found in response");
    }

    // Метод по умолчанию — логин как админ
    public static Task<string> GetJwtTokenAsync(HttpClient client)
    {
        return GetJwtTokenAsync(client, "Admin1@gmail.com", "user123");
    }
}
