using Application.Common.Interfaces;
using Application.CQRS.Posts.DTOs;
using Application.CQRS.Posts.Queries.Requests;
using Common.GlobalResponse;
using MediatR;
using Repository.Repositories;

public class GetPostsByProviderIdHandler : IRequestHandler<GetPostsByProviderIdQuery, ResponseModel<List<PostDto>>>
{
    private readonly IPostRepository _postRepository;
    private readonly IPostLikeRepository _postLikeRepository;
    private readonly IClientProfileRepository _clientProfileRepository;
    private readonly IUserContext _userContext;

    public GetPostsByProviderIdHandler(
        IPostRepository postRepository,
        IPostLikeRepository postLikeRepository,
        IClientProfileRepository clientProfileRepository,
        IUserContext userContext)
    {
        _postRepository = postRepository;
        _postLikeRepository = postLikeRepository;
        _clientProfileRepository = clientProfileRepository;
        _userContext = userContext;
    }

    public async Task<ResponseModel<List<PostDto>>> Handle(GetPostsByProviderIdQuery request, CancellationToken cancellationToken)
    {
        var posts = await _postRepository.GetPostsByProviderIdAsync(request.ServiceProviderProfileId);
        var userId = _userContext.GetCurrentUserId(); // или MustGetUserId(), если всегда авторизован

        int? clientProfileId = null;
        if (userId != null)
        {
            var clientProfile = await _clientProfileRepository.GetByUserIdAsync(userId.Value);
            clientProfileId = clientProfile?.Id;
        }

        var result = new List<PostDto>();

        foreach (var post in posts)
        {
            var likesCount = await _postLikeRepository.GetLikesCountAsync(post.Id);
            bool hasLiked = false;

            if (clientProfileId != null)
                hasLiked = await _postLikeRepository.HasLikedAsync(post.Id, clientProfileId.Value);

            result.Add(new PostDto
            {
                Id = post.Id,
                ServiceProviderProfileId = post.ServiceProviderProfileId,
                Title = post.Title,
                Content = post.Content,
                ImageUrl = post.ImageUrl,
                CreatedAt = post.CreatedAt,
                CreatedBy = post.CreatedBy?.ToString(),
                LikesCount = likesCount,
                HasLiked = hasLiked
            });
        }

        return ResponseModel<List<PostDto>>.Success(result);
    }
}
