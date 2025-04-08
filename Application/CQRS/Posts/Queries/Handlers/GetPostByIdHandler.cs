using Application.Common.Interfaces;
using Application.CQRS.Posts.DTOs;
using AutoMapper;
using Common.Exceptions;
using Common.GlobalResponse;
using Infrastructure.Services;
using MediatR;
using Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Posts.Queries.Handlers;

public class GetPostByIdHandler
{

    public record struct Command (int Id) : IRequest<ResponseModel<GetPostByIdDto>>;


    public sealed class Handler(IUnitOfWork unitWork, IMapper mapper , IActivityLoggerService activityLogger , IUserContext userContext) : IRequestHandler<Command, ResponseModel<GetPostByIdDto>>
    {
        private readonly IUnitOfWork _unitWork = unitWork;
        private readonly IMapper _mapper = mapper;
        private readonly IUserContext _userContext = userContext;
        private readonly IActivityLoggerService _activityLogger = activityLogger;




        public async Task<ResponseModel<GetPostByIdDto>> Handle(Command request, CancellationToken cancellationToken)
        {

            var post  = await _unitWork.PostRepository.GetByIdAsync(request.Id);
            
            if (post == null) throw new BadRequestException("Post does not exist with the provided ID.");
            
            var postDto = _mapper.Map<GetPostByIdDto>(post);

            #region ActivityLog


            var currentUserId = _userContext.GetCurrentUserId();

            await _activityLogger.LogAsync(


            userId: currentUserId.Value,
                    action: "Get",
                    entityType: "Post",
                    entityId: post.Id,
                    performedBy: currentUserId.Value,

                   description: $"Post with ID {post.Id} and Title '{post.Title}' was retrieved by user {currentUserId.Value}."

            );

            #endregion

            return new ResponseModel<GetPostByIdDto>()
            {
                Data = postDto,
                IsSuccess = true
            };



        }
    }

}
