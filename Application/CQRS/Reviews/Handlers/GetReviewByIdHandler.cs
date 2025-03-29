using Application.Common.Interfaces;
using Application.CQRS.Reviews.DTOs;
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

namespace Application.CQRS.Reviews.Handlers;

public class GetReviewByIdHandler
{

    public record struct GetReviewByIdQuery(int Id) : IRequest<ResponseModel<GetReviewByIdDto>>;

    public sealed class Handler(IUnitOfWork unitOfWork, IMapper mapper , IActivityLoggerService activityLogger , IUserContext userContext)
    : IRequestHandler<GetReviewByIdQuery, ResponseModel<GetReviewByIdDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IActivityLoggerService _activityLogger = activityLogger;
        private readonly IUserContext _userContext = userContext;

        public async Task<ResponseModel<GetReviewByIdDto>> Handle(GetReviewByIdQuery request, CancellationToken cancellationToken)
        {
            var review = await _unitOfWork.ReviewRepository.GetByIdAsync(request.Id);
            if (review == null)
                throw new NotFoundException("Review not found.");

            #region ActivityLog

            var currentUserId = _userContext.GetCurrentUserId();
            if (!currentUserId.HasValue)
                throw new UnauthorizedAccessException("User is not authenticated.");

            await _activityLogger.LogAsync(
                userId: currentUserId.Value,
                action: "Get",
                entityType: "Review",
                entityId: review.Id,
                performedBy: currentUserId.Value,
                description: $"User {currentUserId.Value} retrieved review with ID: {review.Id}. Rating: {review.Rating}, Comment: {review.Comment}"
            );

            #endregion



            var responseDto = _mapper.Map<GetReviewByIdDto>(review);


            return new ResponseModel<GetReviewByIdDto>
            {
                Data = responseDto,
                IsSuccess = true,
                Errors = ["Review retrieved successfully."]
            };
        }


    }
}
