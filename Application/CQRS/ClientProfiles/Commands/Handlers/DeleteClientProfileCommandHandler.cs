using Application.CQRS.ClientProfiles.Commands.Requests;
using Application.Common.Interfaces;
using Common.Exceptions;
using Common.GlobalResponse;
using MediatR;
using Repository.Common;

namespace Application.CQRS.ClientProfiles.Commands.Handlers;

public class DeleteClientProfileCommandHandler : IRequestHandler<DeleteClientProfileCommand, ResponseModel<string>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthorizationService _authorizationService;
    private readonly IUserContext _userContext;
    private readonly IActivityLoggerService _activityLogger;

    public DeleteClientProfileCommandHandler(
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

    public async Task<ResponseModel<string>> Handle(DeleteClientProfileCommand request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.ClientProfileRepository.GetByIdAsync(request.Id);
        if (entity is null)
            throw new NotFoundException("Client profile not found.");

        if (entity.IsDeleted)
            throw new ConflictException("Client profile already deleted.");

        _authorizationService.AuthorizeOwnerOrAdmin(
            resourceOwnerId: entity.UserId,
            currentUserId: request.DeletedBy,
            currentUserRole: _userContext.GetUserRole()!
        );

        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        entity.DeletedBy = request.DeletedBy;
        entity.DeletedReason = request.DeletedReason;

        await _unitOfWork.ClientProfileRepository.UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync();

        await _activityLogger.LogAsync(
            userId: entity.UserId,
            action: "Delete",
            entityType: "ClientProfile",
            entityId: entity.Id,
            performedBy: request.DeletedBy,
            description: request.DeletedReason
        );

        return new ResponseModel<string>
        {
            Data = "Client profile deleted successfully.",
            IsSuccess = true
        };
    }
}
