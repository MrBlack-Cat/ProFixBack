using Application.CQRS.Certificates.Commands.Requests;
using Application.Common.Interfaces;
using Common.Exceptions;
using Common.GlobalResponse;
using MediatR;
using Repository.Common;

namespace Application.CQRS.Certificates.Commands.Handlers;

public class DeleteCertificateCommandHandler : IRequestHandler<DeleteCertificateCommand, ResponseModel<string>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthorizationService _authorizationService;
    private readonly IUserContext _userContext;
    private readonly IActivityLoggerService _activityLogger;

    public DeleteCertificateCommandHandler(
        IUnitOfWork unitOfWork,
        IAuthorizationService authorizationService,
        IUserContext userContext,
        IActivityLoggerService activityLogger)
    {
        _unitOfWork = unitOfWork;
        _authorizationService = authorizationService;
        _userContext = userContext;
        _activityLogger = activityLogger;
    }

    public async Task<ResponseModel<string>> Handle(DeleteCertificateCommand request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.CertificateRepository.GetByIdAsync(request.Id);
        if (entity is null || entity.IsDeleted)
            throw new NotFoundException("Certificate not found.");

        if (entity.CreatedBy == null)
            throw new ForbiddenException("Unable to verify owner of this certificate.");

        _authorizationService.AuthorizeOwnerOrAdmin(
            resourceOwnerId: entity.CreatedBy.Value,
            currentUserId: request.DeletedByUserId,
            currentUserRole: _userContext.GetUserRole()
        );


        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        entity.DeletedBy = request.DeletedByUserId;
        entity.DeletedReason = request.Reason;

        await _unitOfWork.CertificateRepository.DeleteAsync(entity);
        await _unitOfWork.SaveChangesAsync();

        await _activityLogger.LogAsync(
            userId: entity.CreatedBy ?? request.DeletedByUserId,
            action: "Delete",
            entityType: "Certificate",
            entityId: entity.Id,
            performedBy: request.DeletedByUserId,
            description: request.Reason
        );

        return new ResponseModel<string> { Data = "Certificate deleted successfully.", IsSuccess = true };
    }
}
