using Application.Common.Interfaces;
using Application.CQRS.Posts.DTOs;
using AutoMapper;
using Common.GlobalResponse;
using Domain.Entities;
using Infrastructure.Services;
using MediatR;
using Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Posts.Queries.Handlers;

public class PostListHandler
{

    public record struct Command :IRequest<ResponseModel<List<PostListDto>>>;


    public sealed class Handler(IUnitOfWork unitWork , IMapper mapper, IActivityLoggerService activityLogger , IUserContext userContext) : IRequestHandler<Command, ResponseModel<List<PostListDto>>>
    {


        private readonly IUnitOfWork _unitWork = unitWork;
        private readonly IMapper _mapper = mapper;
        private readonly IUserContext _userContext = userContext;
        private readonly IActivityLoggerService _activityLogger = activityLogger;

        public async Task<ResponseModel<List<PostListDto>>> Handle(Command request, CancellationToken cancellationToken)
        {

            var posts = await _unitWork.PostRepository.GetAllAsync();

            var postListDto = _mapper.Map<List<PostListDto>>(posts);



            #region ActivityLog

            var currentUserId = _userContext.GetCurrentUserId();

            if (currentUserId.HasValue)
            {
                await _activityLogger.LogAsync(
                    userId: 0,
                    action: "GetList",
                    entityType: "Post",
                    entityId: 0,
                    performedBy: currentUserId.Value,
                    description: $"User {currentUserId.Value} retrieved {posts.Count()} posts."
                );
            }

            #endregion



            return new ResponseModel<List<PostListDto>>()
            {
                Data = postListDto,
                IsSuccess = true
            };  


        }
    }

}
