using Application.CQRS.PostLikes.Commands.Requests;
using Application.Common.Interfaces;
using Common.Exceptions;
using Common.GlobalResponse;
using MediatR;
using Repository.Repositories;

namespace Application.CQRS.PostLikes.Commands.Handlers;

public class UnlikePostCommandHandler : IRequestHandler<UnlikePostCommand, ResponseModel<string>>
{
    private readonly IPostLikeRepository _postLikeRepository;
    private readonly IClientProfileRepository _clientProfileRepository;
    private readonly IUserContext _userContext;

    public UnlikePostCommandHandler(
        IPostLikeRepository postLikeRepository,
        IClientProfileRepository clientProfileRepository,
        IUserContext userContext)
    {
        _postLikeRepository = postLikeRepository;
        _clientProfileRepository = clientProfileRepository;
        _userContext = userContext;
    }

    public async Task<ResponseModel<string>> Handle(UnlikePostCommand request, CancellationToken cancellationToken)
    {
        var userId = _userContext.MustGetUserId();

        var clientProfile = await _clientProfileRepository.GetByUserIdAsync(userId);
        if (clientProfile == null)
            throw new NotFoundException("Client profile not found.");

        var hasLiked = await _postLikeRepository.HasLikedAsync(request.PostId, clientProfile.Id);
        if (!hasLiked)
            return ResponseModel<string>.Fail("You haven’t liked this post yet.");

        await _postLikeRepository.RemoveLikeAsync(request.PostId, clientProfile.Id);

        return ResponseModel<string>.Success("Post unliked successfully");
    }
}
