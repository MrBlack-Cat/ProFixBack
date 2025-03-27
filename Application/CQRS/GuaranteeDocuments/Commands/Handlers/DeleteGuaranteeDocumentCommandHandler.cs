using Application.Common.Interfaces;
using Application.CQRS.GuaranteeDocuments.Commands.Requests;
using Common.Exceptions;
using Common.GlobalResponse;
using MediatR;
using Repository.Common;

namespace Application.CQRS.GuaranteeDocuments.Commands.Handlers;

public class DeleteGuaranteeDocumentCommandHandler : IRequestHandler<DeleteGuaranteeDocumentCommand, ResponseModel<string>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;
    private readonly IAuthorizationService _authorizationService;
    private readonly IActivityLoggerService _activityLogger;

    public DeleteGuaranteeDocumentCommandHandler(
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

    public async Task<ResponseModel<string>> Handle(DeleteGuaranteeDocumentCommand request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.GuaranteeDocumentRepository.GetByIdAsync(request.Id);
        if (entity is null || entity.IsDeleted)
            throw new NotFoundException("Guarantee document not found.");

        if (entity.CreatedBy == null)
            throw new ForbiddenException("Unable to verify document ownership.");

        _authorizationService.AuthorizeOwnerOrAdmin(
            resourceOwnerId: entity.CreatedBy.Value,
            currentUserId: request.DeletedByUserId,
            currentUserRole: _userContext.GetUserRole()
        );

        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        entity.DeletedBy = request.DeletedByUserId;
        entity.DeletedReason = request.Reason;

        await _unitOfWork.GuaranteeDocumentRepository.UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync();

        await _activityLogger.LogAsync(
            userId: entity.CreatedBy.Value,
            action: "Delete",
            entityType: "GuaranteeDocument",
            entityId: entity.Id,
            performedBy: request.DeletedByUserId,
            description: request.Reason
        );

        return new ResponseModel<string> { Data = "Guarantee document deleted successfully.", IsSuccess = true };
    }
}
