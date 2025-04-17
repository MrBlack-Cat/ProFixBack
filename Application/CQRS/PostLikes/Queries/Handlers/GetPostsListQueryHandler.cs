using Application.CQRS.Posts.DTOs;
using Common.GlobalResponse;
using MediatR;
using Repository.Common;
using Repository.Repositories;
using Application.CQRS.Posts.Queries.Requests;
using Application.Common.Interfaces;

namespace Application.CQRS.Posts.Queries.Handlers;

public class GetPostsListQueryHandler : IRequestHandler<GetPostsListQuery, ResponseModel<List<PostDto>>>
{
    private readonly IPostRepository _postRepository;
    private readonly IPostLikeRepository _postLikeRepository;
    private readonly IClientProfileRepository _clientProfileRepository;
    private readonly IUserContext _userContext;

    public GetPostsListQueryHandler(
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

    public async Task<ResponseModel<List<PostDto>>> Handle(GetPostsListQuery request, CancellationToken cancellationToken)
    {
        var posts = await _postRepository.GetAllAsync();
        var userId = _userContext.GetCurrentUserId(); 
        int? clientProfileId = null;

        if (userId != null)
        {
            var clientProfile = await _clientProfileRepository.GetByUserIdAsync(userId.Value);
            clientProfileId = clientProfile?.Id;
        }

        var postDtos = new List<PostDto>();

        foreach (var post in posts)
        {
            var likesCount = await _postLikeRepository.GetLikesCountAsync(post.Id);
            bool hasLiked = false;

            if (clientProfileId != null)
                hasLiked = await _postLikeRepository.HasLikedAsync(post.Id, clientProfileId.Value);

            postDtos.Add(new PostDto
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                ImageUrl = post.ImageUrl,
                CreatedAt = post.CreatedAt,
                LikesCount = likesCount,
                HasLiked = hasLiked
            });
        }

        return ResponseModel<List<PostDto>>.Success(postDtos);
    }
}
