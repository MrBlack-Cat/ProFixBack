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

public class UpdateReviewHandler
{
    public record struct UpdateReviewCommand(UpdateReviewDto Dto) : IRequest<ResponseModel<UpdateReviewDto>>;


    public sealed class Handler(IUnitOfWork unitOfWork, IMapper mapper , IActivityLoggerService activityLogger , IUserContext userContext)
    : IRequestHandler<UpdateReviewCommand, ResponseModel<UpdateReviewDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IActivityLoggerService _activityLogger = activityLogger;
        private readonly IUserContext _userContext = userContext;


        public async Task<ResponseModel<UpdateReviewDto>> Handle(UpdateReviewCommand request, CancellationToken cancellationToken)
        {
            var review = await _unitOfWork.ReviewRepository.GetByIdAsync(request.Dto.Id);
            if (review == null)
                throw new NotFoundException("Review not found.");

            // Məlumatları yeniləyirik
            _mapper.Map(request.Dto, review);

            await _unitOfWork.ReviewRepository.UpdateAsync(review);
            await _unitOfWork.SaveChangesAsync();


            #region ActivityLog


            var currentUserId = _userContext.GetCurrentUserId();
            if (!currentUserId.HasValue)
                throw new UnauthorizedAccessException("User is not authenticated.");


            await _activityLogger.LogAsync(
                    userId: currentUserId.Value,
                    action: "Update",
                    entityType: "Review",
                    entityId: review.Id,
                    performedBy: currentUserId.Value,
                    description: $"User {currentUserId.Value} updated review with ID: {review.Id}. Rating: {review.Rating}, Comment: {review.Comment}"
                );

            #endregion

            var responseDto = _mapper.Map<UpdateReviewDto>(review);

            return new ResponseModel<UpdateReviewDto>
            {
                Data = responseDto,
                IsSuccess = true,
                Errors =[]
            };
        }
    }


}
