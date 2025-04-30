using Common.DTOs;
using Common.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Repository.Common;
using System.Text;
using System.Text.Json;

namespace Infrastructure.Services;

public partial class ChatService : IChatService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly HttpClient _httpClient;
    private readonly string _openRouterApiKey;
    private bool _isInitialized;
    private readonly QuestionRouter _questionRouter = new();
    private readonly QuestionIntentAnalyzer _questionIntentAnalyzer = new();
    private readonly FrontendHelpService _frontendHelpService = new();


    private List<ServiceBooking> _bookings = new();
    private List<Post> _posts = new();
    private List<PostLike> _postLikes = new();
    private List<Certificate> _certificates = new();
    private List<GuaranteeDocument> _guarantees = new();
    private List<ClientProfile> _clients = new();
    private List<ServiceProviderProfile> _providers = new();

    public ChatService(IUnitOfWork unitOfWork, IConfiguration configuration, HttpClient httpClient)
    {
        _unitOfWork = unitOfWork;
        _openRouterApiKey = configuration["OpenRouter:ApiKey"]!;
        _httpClient = httpClient;

        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_openRouterApiKey}");
        _httpClient.DefaultRequestHeaders.Add("HTTP-Referer", "https://openrouter.ai");
        _httpClient.DefaultRequestHeaders.Add("X-Title", "ProFixBot");
    }

    public async Task InitializeAsync()
    {
        if (_isInitialized) return;

        _bookings = (await _unitOfWork.ServiceBookingRepository.GetAllBookingsAsync()).ToList();
        _posts = (await _unitOfWork.PostRepository.GetAllPostsAsync()).ToList();
        _postLikes = (await _unitOfWork.PostLikeRepository.GetAllPostLikesAsync()).ToList();
        _certificates = (await _unitOfWork.CertificateRepository.GetAllCertificatesAsync()).ToList();
        _guarantees = (await _unitOfWork.GuaranteeDocumentRepository.GetAllAsync()).ToList();
        _clients = (await _unitOfWork.ClientProfileRepository.GetAllAsync()).ToList();
        _providers = (await _unitOfWork.ServiceProviderProfileRepository.GetAllAsync()).ToList();

        _isInitialized = true;
    }

    public async Task<string> GetBotResponseAsync(int userId, string userMessage, string? sessionId = null)
    {
        await InitializeAsync();

        if (userId == 0 && !string.IsNullOrEmpty(sessionId))
        {
            return "Вы гость. Пожалуйста, зарегистрируйтесь для получения расширенных возможностей.";
        }

        var topic = _questionRouter.DetectTopic(userMessage);
        var intent = _questionIntentAnalyzer.DetectIntent(userMessage);

        if (topic == QuestionRouter.QuestionTopic.Frontend)
        {
            var frontendIntent = _frontendHelpService.FindHelp(userMessage);

            if (frontendIntent != null)
            {
                return $"{frontendIntent.Description}\n\n➡️ Путь: {frontendIntent.TargetPath}";
            }
            else
            {
                return await GetFrontendHelpResponseAsync(userMessage);
            }
        }

        if (topic == QuestionRouter.QuestionTopic.Unknown && intent == QuestionIntentAnalyzer.QuestionIntent.Global)
        {
            return await GetGlobalBotResponseAsync(userId, userMessage);
        }

        return (topic, intent) switch
        {
            (QuestionRouter.QuestionTopic.Bookings, _) => await GetBookingBotResponseAsync(userId, userMessage),
            (QuestionRouter.QuestionTopic.Posts, _) => await GetPostsBotResponseAsync(userId, userMessage),
            (QuestionRouter.QuestionTopic.Certificates, _) => await GetCertificatesBotResponseAsync(userId, userMessage),
            (QuestionRouter.QuestionTopic.Guarantees, _) => await GetGuaranteesBotResponseAsync(userId, userMessage),
            _ => "Извините, я не смог определить, о чём именно ваш вопрос. Пожалуйста, уточните его."
        };
    }




    public async Task<IEnumerable<ChatMessageDto>> GetMessagesByUserIdAsync(int userId)
    {
        var messages = await _unitOfWork.ChatMessageRepository.GetMessagesByUserIdAsync(userId);

        return messages.Select(m => new ChatMessageDto
        {
            Id = m.Id,
            UserId = m.UserId,
            SessionId = m.SessionId,
            IsGuest = m.IsGuest,
            Sender = m.Sender,
            MessageText = m.MessageText,
            CreatedAt = m.CreatedAt
        });
    }
}
