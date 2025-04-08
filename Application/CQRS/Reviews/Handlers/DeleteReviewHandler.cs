using Application.Common.Interfaces;
using AutoMapper;
using Common.Exceptions;
using Common.GlobalResponse;
using MediatR;
using Repository.Common;

namespace Application.CQRS.Reviews.Handlers;

public class DeleteReviewHandler
{
    public record struct DeleteReviewCommand(int Id, int DeletedByUserId, string Reason)
        : IRequest<ResponseModel<string>>;

    public sealed class Handler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IActivityLoggerService activityLogger)
        : IRequestHandler<DeleteReviewCommand, ResponseModel<string>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IActivityLoggerService _activityLogger = activityLogger;

        public async Task<ResponseModel<string>> Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
        {
            var review = await _unitOfWork.ReviewRepository.GetByIdAsync(request.Id);
            if (review == null || review.IsDeleted)
                throw new NotFoundException("Review not found.");

            review.IsDeleted = true;
            review.DeletedAt = DateTime.UtcNow;
            review.DeletedBy = request.DeletedByUserId;
            review.DeletedReason = request.Reason;

            await _unitOfWork.ReviewRepository.UpdateAsync(review);
            await _unitOfWork.SaveChangesAsync();

            await _activityLogger.LogAsync(
                userId: review.ClientProfileId,
                action: "Delete",
                entityType: "Review",
                entityId: review.Id,
                performedBy: request.DeletedByUserId,
                description: $"Review deleted. Rating: {review.Rating}, Comment: {review.Comment}. Reason: {request.Reason}");

            return new ResponseModel<string>
            {
                Data = "Review deleted successfully.",
                IsSuccess = true
            };
        }
    }
}
