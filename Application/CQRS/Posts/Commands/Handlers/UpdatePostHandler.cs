using Application.Common.Interfaces;
using Application.CQRS.Posts.DTOs;
using AutoMapper;
using Common.Exceptions;
using Common.GlobalResponse;
using MediatR;
using Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Posts.Commands.Handlers;

public class UpdatePostHandler
{

    public class UpdatePostCommand : IRequest<ResponseModel<UpdatePostDto>>
    {
        public UpdatePostDto PostDto { get; set; } = null!;
    }

    public class Handler(IUnitOfWork unitWork, IMapper mapper , IActivityLoggerService activityLogger , IUserContext userContext ) : IRequestHandler<UpdatePostCommand, ResponseModel<UpdatePostDto>>
    {

        private readonly IUnitOfWork _unitWork = unitWork;
        private readonly IMapper _mapper = mapper;
        private readonly IUserContext _userContext = userContext;
        private readonly IActivityLoggerService _activityLogger = activityLogger;

        public async Task<ResponseModel<UpdatePostDto>> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
        {

            var post = await _unitWork.PostRepository.GetByIdAsync(request.PostDto.Id); 

            if(post == null) throw new NotFoundException("Post does not exist with the provided ID.");

            _mapper.Map(request.PostDto, post); 
            post.UpdatedAt = DateTime.UtcNow;

            await _unitWork.PostRepository.UpdateAsync(post);
            await _unitWork.SaveChangesAsync();


            #region ActivityLog

            var currentUserId = _userContext.GetCurrentUserId();

            await _activityLogger.LogAsync(
                userId: currentUserId.Value,
                action: "Update",
                entityType: "Post",
                entityId: post.Id, 
                performedBy: currentUserId,
                description: $"User {currentUserId} updated {post.Title} posts."
            );


            #endregion


            var response = _mapper.Map<UpdatePostDto>(post);

            return new ResponseModel<UpdatePostDto>
            {
                Data = response,
                IsSuccess = true
            };


        }
    }


}
