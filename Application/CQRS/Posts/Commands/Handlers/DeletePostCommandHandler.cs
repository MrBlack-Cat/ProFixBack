using Application.Common.Interfaces;
using Common.Exceptions;
using Common.GlobalResponse;
using MediatR;
using Repository.Common;

namespace Application.CQRS.Posts.Commands.Handlers;

public class DeletePostCommandHandler
{
    public record Command(int Id, int DeletedBy, string DeletedReason) : IRequest<ResponseModel<string>>;

    public sealed class Handler(
        IUnitOfWork unitOfWork,
        IUserContext userContext,
        IActivityLoggerService activityLogger)
        : IRequestHandler<Command, ResponseModel<string>>
    {
        public async Task<ResponseModel<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var post = await unitOfWork.PostRepository.GetByIdAsync(request.Id);
            if (post == null)
                throw new NotFoundException("Post not found.");

            post.IsDeleted = true;
            post.DeletedAt = DateTime.UtcNow;
            post.DeletedBy = request.DeletedBy;
            post.DeletedReason = request.DeletedReason;
            post.IsActive = false;

            await unitOfWork.PostRepository.DeleteAsync(post);
            await unitOfWork.SaveChangesAsync();

            await activityLogger.LogAsync(
                userId: request.DeletedBy,
                action: "Delete",
                entityType: "Post",
                entityId: post.Id,
                performedBy: request.DeletedBy,
                description: $"Post '{post.Title}' deleted. Reason: {request.DeletedReason}");

            return new ResponseModel<string>
            {
                Data = "Post deleted successfully.",
                IsSuccess = true
            };
        }
    }
}
