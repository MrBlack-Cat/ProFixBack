using Application.CQRS.PostLikes.Queries.Requests;
using Common.GlobalResponse;
using MediatR;
using Repository.Repositories;

namespace Application.CQRS.PostLikes.Queries.Handlers;

public class GetPostLikesCountQueryHandler : IRequestHandler<GetPostLikesCountQuery, ResponseModel<int>>
{
    private readonly IPostLikeRepository _postLikeRepository;

    public GetPostLikesCountQueryHandler(IPostLikeRepository postLikeRepository)
    {
        _postLikeRepository = postLikeRepository;
    }

    public async Task<ResponseModel<int>> Handle(GetPostLikesCountQuery request, CancellationToken cancellationToken)
    {
        var count = await _postLikeRepository.GetLikesCountAsync(request.PostId);
        return ResponseModel<int>.Success(count);
    }
}
