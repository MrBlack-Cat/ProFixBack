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
    public record struct UpdateReviewCommand(int Id, int UpdatedByUserId, UpdateReviewDto Dto)
        : IRequest<ResponseModel<UpdateReviewDto>>;

    public sealed class Handler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IActivityLoggerService activityLogger,
        IUserContext userContext)
        : IRequestHandler<UpdateReviewCommand, ResponseModel<UpdateReviewDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IActivityLoggerService _activityLogger = activityLogger;
        private readonly IUserContext _userContext = userContext;

        public async Task<ResponseModel<UpdateReviewDto>> Handle(UpdateReviewCommand request, CancellationToken cancellationToken)
        {
            var review = await _unitOfWork.ReviewRepository.GetByIdAsync(request.Id);
            if (review == null || review.IsDeleted)
                throw new NotFoundException("Review not found.");

            _mapper.Map(request.Dto, review);
            review.UpdatedAt = DateTime.UtcNow;
            review.UpdatedBy = request.UpdatedByUserId;

            await _unitOfWork.ReviewRepository.UpdateAsync(review);
            await _unitOfWork.SaveChangesAsync();

            await _activityLogger.LogAsync(
                userId: review.ClientProfileId,
                action: "Update",
                entityType: "Review",
                entityId: review.Id,
                performedBy: request.UpdatedByUserId,
                description: $"User {request.UpdatedByUserId} updated review {review.Id}: Rating = {review.Rating}, Comment = {review.Comment}");

            var responseDto = _mapper.Map<UpdateReviewDto>(review);

            return new ResponseModel<UpdateReviewDto>
            {
                Data = responseDto,
                IsSuccess = true
            };
        }
    }
}
