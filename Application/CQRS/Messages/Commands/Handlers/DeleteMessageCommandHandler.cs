using Application.CQRS.Messages.Commands.Requests;
using Application.Common.Interfaces;
using Common.Exceptions;
using Common.GlobalResponse;
using MediatR;
using Repository.Common;

namespace Application.CQRS.Messages.Commands.Handlers;

public class DeleteMessageCommandHandler : IRequestHandler<DeleteMessageCommand, ResponseModel<string>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;
    private readonly IAuthorizationService _authorizationService;
    private readonly IActivityLoggerService _activityLogger;

    public DeleteMessageCommandHandler(
        IUnitOfWork unitOfWork,
        IUserContext userContext,
        IAuthorizationService authorizationService,
        IActivityLoggerService activityLogger)
    {
        _unitOfWork = unitOfWork;
        _userContext = userContext;
        _authorizationService = authorizationService;
        _activityLogger = activityLogger;
    }

    public async Task<ResponseModel<string>> Handle(DeleteMessageCommand request, CancellationToken cancellationToken)
    {
        var message = await _unitOfWork.MessageRepository.GetByIdAsync(request.Id);
        if (message is null || message.IsDeleted)
            throw new NotFoundException("Message not found.");

        if (message.CreatedBy == null)
            throw new ForbiddenException("Cannot verify message ownership.");

        _authorizationService.AuthorizeOwnerOrAdmin(
            resourceOwnerId: message.CreatedBy.Value,
            currentUserId: request.DeletedByUserId,
            currentUserRole: _userContext.GetUserRole()
        );

        message.IsDeleted = true;
        message.DeletedAt = DateTime.UtcNow;
        message.DeletedBy = request.DeletedByUserId;
        message.DeletedReason = request.Reason;

        await _unitOfWork.MessageRepository.UpdateAsync(message);
        await _unitOfWork.SaveChangesAsync();

        await _activityLogger.LogAsync(
            userId: message.CreatedBy ?? request.DeletedByUserId,
            action: "Delete",
            entityType: "Message",
            entityId: message.Id,
            performedBy: request.DeletedByUserId,
            description: request.Reason
        );

        return new ResponseModel<string> { Data = "Message deleted successfully.", IsSuccess = true };
    }
}
