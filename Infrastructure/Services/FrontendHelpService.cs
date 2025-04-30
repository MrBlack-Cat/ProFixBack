using Common.DTOs;
using System.Text.Json;

namespace Infrastructure.Services;

public class FrontendHelpService
{
    private List<FrontendIntent> _intents = new();

    public FrontendHelpService()
    {
        LoadIntents();
    }

    private void LoadIntents()
    {
        var jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FrontendHelpData", "frontend-intents.json");
        if (File.Exists(jsonPath))
        {
            var json = File.ReadAllText(jsonPath);
            _intents = JsonSerializer.Deserialize<List<FrontendIntent>>(json) ?? new();
        }
    }

    public FrontendIntent? FindHelp(string userQuery)
    {
        var query = userQuery.ToLower();

        return _intents.FirstOrDefault(intent =>
            query.Contains(intent.Intent.ToLower()) ||
            query.Contains(intent.Description.ToLower()));
    }
}
