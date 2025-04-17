using Application.CQRS.Posts.DTOs;
using Application.CQRS.Posts.Queries.Requests;
using Common.GlobalResponse;
using Domain.Entities;
using MediatR;
using Repository.Repositories;

namespace Application.CQRS.Posts.Queries.Handlers
{
    public class GetPostsByLikedQueryHandler : IRequestHandler<GetPostsByLikedQuery, ResponseModel<List<PostDto>>>
    {
        private readonly IPostRepository _postRepository;
        private readonly IPostLikeRepository _postLikeRepository;

        public GetPostsByLikedQueryHandler(IPostRepository postRepository, IPostLikeRepository postLikeRepository)
        {
            _postRepository = postRepository;
            _postLikeRepository = postLikeRepository;
        }

        public async Task<ResponseModel<List<PostDto>>> Handle(GetPostsByLikedQuery request, CancellationToken cancellationToken)
        {
            var posts = await _postRepository.GetPostsByLikedAsync();

            var postDtos = new List<PostDto>();

            foreach (var post in posts)
            {
                var likesCount = await _postLikeRepository.GetLikesCountAsync(post.Id);

                var postDto = new PostDto
                {
                    Id = post.Id,
                    ServiceProviderProfileId = post.ServiceProviderProfileId,
                    Title = post.Title,
                    Content = post.Content,
                    ImageUrl = post.ImageUrl,
                    CreatedBy = post.CreatedBy?.ToString(),
                    CreatedAt = post.CreatedAt,
                    LikesCount = likesCount,
                    HasLiked = false
                };

                postDtos.Add(postDto);
            }

            return ResponseModel<List<PostDto>>.Success(postDtos);
        }
    }
}
