using Application.Common.Interfaces;
using Application.CQRS.Reviews.DTOs;
using AutoMapper;
using Common.GlobalResponse;
using MediatR;
using Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Reviews.Handlers;

public class ReviewListHandler
{

    public record struct GetReviewListQuery()
    : IRequest<ResponseModel<List<ReviewListDto>>>;


    public sealed class GetReviewListHandler(IUnitOfWork unitOfWork, IMapper mapper , IActivityLoggerService activityLogger , IUserContext userContext )
    : IRequestHandler<GetReviewListQuery, ResponseModel<List<ReviewListDto>>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IActivityLoggerService _activityLogger = activityLogger;
        private readonly IUserContext _userContext = userContext;



        public async Task<ResponseModel<List<ReviewListDto>>> Handle(GetReviewListQuery request, CancellationToken cancellationToken)
        {
            var reviews = await _unitOfWork.ReviewRepository.GetAllAsync();

            #region ActiviyLog


            var currentUserId = _userContext.GetCurrentUserId();
            if (!currentUserId.HasValue)
                throw new UnauthorizedAccessException("User is not authenticated.");

            await _activityLogger.LogAsync(
                userId: 0,
                action: "GetList",
                entityType: "Review",
                entityId: reviews.Count(), 
                performedBy: currentUserId.Value,
                description: $"User {currentUserId.Value} retrieved {reviews.Count()} reviews."
            );

            #endregion



            if (!reviews.Any())
                return new ResponseModel<List<ReviewListDto>>
                {
                    Data = new List<ReviewListDto>(),
                    IsSuccess = false,
                    Errors = ["No reviews found."]
                };

            var responseDto = _mapper.Map<List<ReviewListDto>>(reviews);

            return new ResponseModel<List<ReviewListDto>>
            {
                Data = responseDto,
                IsSuccess = true,
                Errors =[]
            };
        }
    }



}
