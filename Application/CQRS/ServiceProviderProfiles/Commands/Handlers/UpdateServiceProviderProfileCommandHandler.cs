using Application.Common.Interfaces;
using Application.CQRS.ServiceProviderProfiles.Commands.Requests;
using Application.CQRS.ServiceProviderProfiles.DTOs;
using AutoMapper;
using Common.Exceptions;
using Common.GlobalResponse;
using Domain.Entities;
using Domain.Types;
using MediatR;
using Repository.Common;

namespace Application.CQRS.ServiceProviderProfiles.Commands.Handlers;

public class UpdateServiceProviderProfileCommandHandler
    : IRequestHandler<UpdateServiceProviderProfileCommand, ResponseModel<UpdateServiceProviderProfileDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IActivityLoggerService _activityLogger;

    public UpdateServiceProviderProfileCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IActivityLoggerService activityLogger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _activityLogger = activityLogger;
    }

    public async Task<ResponseModel<UpdateServiceProviderProfileDto>> Handle(UpdateServiceProviderProfileCommand request, CancellationToken cancellationToken)
    {
        var profile = await _unitOfWork.ServiceProviderProfileRepository.GetByIdAsync(request.Id);
        if (profile is null)
            throw new NotFoundException("ServiceProvider profile not found");

        profile.Name = request.Profile.Name;
        profile.Surname = request.Profile.Surname;
        profile.City = request.Profile.City;
        profile.Age = request.Profile.Age;
        profile.GenderId = request.Profile.GenderId;
        profile.ExperienceYears = request.Profile.ExperienceYears;
        //profile.Description = request.Profile.Description;
        profile.IsActive = request.Profile.IsActive;
        profile.ApprovalDate = request.Profile.ApprovalDate;
        profile.UpdatedAt = DateTime.UtcNow;
        profile.UpdatedBy = request.UpdatedBy;

        await _unitOfWork.ServiceProviderProfileRepository.UpdateAsync(profile);

        if (request.Profile.ServiceTypeIds is not null && request.Profile.ServiceTypeIds.Any())
        {
            
            await _unitOfWork.ServiceProviderServiceTypeRepository.DeleteAllByServiceProviderProfileIdAsync(profile.Id);

            foreach (var serviceTypeId in request.Profile.ServiceTypeIds)
            {
                var link = new ServiceProviderServiceType
                {
                    ServiceProviderProfileId = profile.Id,
                    ServiceTypeId = serviceTypeId
                };
                await _unitOfWork.ServiceProviderServiceTypeRepository.AddAsync(link);
            }
        }

        await _unitOfWork.SaveChangesAsync();

        await _activityLogger.LogAsync(
            userId: request.UpdatedBy,
            action: "Update",
            entityType: "ServiceProviderProfile",
            entityId: profile.Id,
            performedBy: request.UpdatedBy,
            description: $"ServiceProvider profile updated. Types count: {request.Profile.ServiceTypeIds?.Count ?? 0}"
        );

        var result = _mapper.Map<UpdateServiceProviderProfileDto>(profile);

        return new ResponseModel<UpdateServiceProviderProfileDto>
        {
            Data = result,
            IsSuccess = true
        };
    }
}
