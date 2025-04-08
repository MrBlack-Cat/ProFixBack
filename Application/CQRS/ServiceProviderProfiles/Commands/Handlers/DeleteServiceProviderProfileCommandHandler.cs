using Application.Common.Interfaces;
using Application.CQRS.ServiceProviderProfiles.Commands.Requests;
using AutoMapper;
using Common.Exceptions;
using Common.GlobalResponse;
using MediatR;
using Repository.Common;



namespace Application.CQRS.ServiceProviderProfiles.Commands.Handlers;

public class DeleteServiceProviderProfileCommandHandler
    : IRequestHandler<DeleteServiceProviderProfileCommand, ResponseModel<string>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IActivityLoggerService _activityLogger;

    public DeleteServiceProviderProfileCommandHandler(
        IUnitOfWork unitOfWork,
        IActivityLoggerService activityLogger)
    {
        _unitOfWork = unitOfWork;
        _activityLogger = activityLogger;
    }

    public async Task<ResponseModel<string>> Handle(DeleteServiceProviderProfileCommand command, CancellationToken cancellationToken)
    {
        var profile = await _unitOfWork.ServiceProviderProfileRepository.GetByIdAsync(command.Id);
        if (profile is null)
            throw new NotFoundException("ServiceProviderProfile not found");

        profile.IsDeleted = true;
        profile.IsActive = false;
        profile.DeletedAt = DateTime.UtcNow;
        profile.DeletedBy = command.DeletedBy;
        profile.DeletedReason = command.DeletedReason;

        await _unitOfWork.ServiceProviderProfileRepository.UpdateAsync(profile);
        await _unitOfWork.SaveChangesAsync();

        await _activityLogger.LogAsync(
            userId: command.DeletedBy,
            action: "Delete",
            entityType: "ServiceProviderProfile",
            entityId: profile.Id,
            performedBy: command.DeletedBy
        );

        return new ResponseModel<string>
        {
            IsSuccess = true,
            Data = "Service provider profile deleted successfully"
        };
    }

}
