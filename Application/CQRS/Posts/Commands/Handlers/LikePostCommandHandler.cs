//using Application.CQRS.Notifications.DTOs;
//using Application.CQRS.Posts.Commands.Requests;
//using Application.Common.Interfaces;
//using Common.Exceptions;
//using Common.GlobalResponse;
//using Domain.Constants;
//using MediatR;
//using Repository.Common;
//using Repository.Repositories;

//namespace Application.CQRS.Posts.Commands.Handlers;

//public class LikePostCommandHandler : IRequestHandler<LikePostCommand, ResponseModel<string>>
//{
//    private readonly IUnitOfWork _unitOfWork;
//    private readonly IPostRepository _postRepository;
//    private readonly IPostLikeRepository _postLikeRepository;
//    private readonly IClientProfileRepository _clientProfileRepository;
//    private readonly INotificationService _notificationService;

//    public LikePostCommandHandler(
//        IUnitOfWork unitOfWork,
//        IPostRepository postRepository,
//        IPostLikeRepository postLikeRepository,
//        IClientProfileRepository clientProfileRepository,
//        INotificationService notificationService)
//    {
//        _unitOfWork = unitOfWork;
//        _postRepository = postRepository;
//        _postLikeRepository = postLikeRepository;
//        _clientProfileRepository = clientProfileRepository;
//        _notificationService = notificationService;
//    }

//    public async Task<ResponseModel<string>> Handle(LikePostCommand request, CancellationToken cancellationToken)
//    {
//        var post = await _postRepository.GetByIdAsync(request.PostId);
//        if (post is null)
//            throw new NotFoundException("Post not found");

//        var clientProfile = await _clientProfileRepository.GetByIdAsync(request.ClientProfileId);
//        if (clientProfile is null)
//            throw new NotFoundException("Client profile not found");

//        var alreadyLiked = await _postLikeRepository.HasLikedAsync(request.PostId, request.ClientProfileId);
//        if (alreadyLiked)
//            return ResponseModel<string>.Fail("You have already liked this post.");

//        await _postLikeRepository.AddLikeAsync(request.PostId, request.ClientProfileId);

//        if (post.CreatedBy is null)
//            throw new Exception("Post does not have a CreatedBy user assigned.");

//        await _notificationService.CreateAsync(new CreateNotificationDto
//        {
//            UserId = post.CreatedBy.Value,
//            CreatedBy = clientProfile.UserId,
//            TypeId = NotificationTypeConstants.LikePost,
//            Message = $"{clientProfile.Name} {clientProfile.Surname} liked your post!"
//        });


//        return ResponseModel<string>.Success("Post liked successfully.");
//    }
//}
