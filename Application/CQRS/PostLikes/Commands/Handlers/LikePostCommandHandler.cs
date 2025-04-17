using Application.CQRS.PostLikes.Commands.Requests;
using Application.Common.Interfaces;
using Common.Exceptions;
using Common.GlobalResponse;
using Domain.Constants;
using Domain.Entities;
using MediatR;
using Repository.Repositories;

namespace Application.CQRS.PostLikes.Commands.Handlers;

public class LikePostCommandHandler : IRequestHandler<LikePostCommand, ResponseModel<string>>
{
    private readonly IPostLikeRepository _postLikeRepository;
    private readonly IPostRepository _postRepository;
    private readonly IClientProfileRepository _clientProfileRepository;
    private readonly IUserContext _userContext;
    private readonly INotificationService _notificationService;

    public LikePostCommandHandler(
        IPostLikeRepository postLikeRepository,
        IPostRepository postRepository,
        IClientProfileRepository clientProfileRepository,
        IUserContext userContext,
        INotificationService notificationService)
    {
        _postLikeRepository = postLikeRepository;
        _postRepository = postRepository;
        _clientProfileRepository = clientProfileRepository;
        _userContext = userContext;
        _notificationService = notificationService;
    }

    public async Task<ResponseModel<string>> Handle(LikePostCommand request, CancellationToken cancellationToken)
    {
        var userId = _userContext.MustGetUserId();

        var clientProfile = await _clientProfileRepository.GetByUserIdAsync(userId);
        if (clientProfile == null)
            throw new NotFoundException("Client profile not found.");

        var hasLiked = await _postLikeRepository.HasLikedAsync(request.PostId, clientProfile.Id);
        if (hasLiked)
            return ResponseModel<string>.Fail("You already liked this post.");

        await _postLikeRepository.AddLikeAsync(request.PostId, clientProfile.Id);

        var post = await _postRepository.GetByIdAsync(request.PostId);
        if (post == null || post.CreatedBy == null)
            throw new NotFoundException("Post or its author not found.");

        await _notificationService.CreateAsync(
            receiverUserId: post.CreatedBy.Value,
            typeId: NotificationTypeConstants.LikePost,
            message: $"{clientProfile.Name} {clientProfile.Surname} liked your post! ❤️",
            createdBy: clientProfile.UserId
        );

        return ResponseModel<string>.Success("Post liked and notification sent");
    }
}
