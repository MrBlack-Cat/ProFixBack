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

public class DeleteReviewHandler
{

    public record struct DeleteReviewCommand(int Id, int? DeletedByUserId, string? Reason) : IRequest<ResponseModel<DeleteReviewDto>>;

    public sealed class Handler(IUnitOfWork unitOfWork, IMapper mapper , IActivityLoggerService activityLogger)
    : IRequestHandler<DeleteReviewCommand, ResponseModel<DeleteReviewDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IActivityLoggerService _activityLogger = activityLogger;

        public async Task<ResponseModel<DeleteReviewDto>> Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
        {
            var review = await _unitOfWork.ReviewRepository.GetByIdAsync(request.Id);
            if (review == null)
                throw new NotFoundException("Review not found.");

            var deleteDto = new DeleteReviewDto
            {
                Id = review.Id,
                DeletedByUserId = request.DeletedByUserId,
                Reason = request.Reason
            };

            await _unitOfWork.ReviewRepository.DeleteAsync(review);
            await _unitOfWork.SaveChangesAsync();


            #region ActivityLog


            await _activityLogger.LogAsync(
                     userId: request.DeletedByUserId.Value,  
                     action: "Delete",
                     entityType: "Review",
                     entityId: review.Id,  
                     performedBy: request.DeletedByUserId,  
                     description: $"Review for ClientProfileId {review.ClientProfileId} and ServiceProviderProfileId {review.ServiceProviderProfileId} deleted. " +
                                  $"Rating: {review.Rating}, Comment: {review.Comment}. Reason: {request.Reason}."
                 );



            #endregion

            return new ResponseModel<DeleteReviewDto>
            {
                Data = deleteDto,
                IsSuccess = true,
                Errors = ["Review deleted successfully."]
            };
        }


    }
}
