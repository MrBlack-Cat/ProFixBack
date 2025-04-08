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

public class CreateServiceProviderProfileCommandHandler
    : IRequestHandler<CreateServiceProviderProfileCommand, ResponseModel<CreateServiceProviderProfileDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IActivityLoggerService _activityLogger;

    public CreateServiceProviderProfileCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IActivityLoggerService activityLogger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _activityLogger = activityLogger;
    }

    public async Task<ResponseModel<CreateServiceProviderProfileDto>> Handle(CreateServiceProviderProfileCommand request, CancellationToken cancellationToken)
    {
        var existing = await _unitOfWork.ServiceProviderProfileRepository.GetByUserIdAsync(request.UserId);
        if (existing is not null)
            throw new ConflictException("Service Provider profile already exists for this user.");

        var profile = new ServiceProviderProfile
        {
            UserId = request.UserId,
            Name = request.Profile.Name,
            Surname = request.Profile.Surname,
            City = request.Profile.City,
            Age = request.Profile.Age,
            GenderId = request.Profile.GenderId,
            ExperienceYears = request.Profile.ExperienceYears,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = request.UserId,
            IsActive = true,
            ParentCategoryId = request.Profile.ParentCategoryId,

        };

        await _unitOfWork.ServiceProviderProfileRepository.AddAsync(profile);
        await _unitOfWork.SaveChangesAsync(); 

        if (request.Profile.ServiceTypeIds is not null && request.Profile.ServiceTypeIds.Any())
        {
            var serviceTypeLinks = request.Profile.ServiceTypeIds.Select(typeId => new ServiceProviderServiceType
            {
                ServiceProviderProfileId = profile.Id,
                ServiceTypeId = typeId
            }).ToList();

            await _unitOfWork.ServiceProviderServiceTypeRepository.AddRangeAsync(serviceTypeLinks);
        }

        await _unitOfWork.SaveChangesAsync();

        await _activityLogger.LogAsync(
            userId: request.UserId,
            action: "Create",
            entityType: "ServiceProviderProfile",
            entityId: profile.Id,
            performedBy: request.UserId,
            description: $"Created ServiceProviderProfile with {request.Profile.ServiceTypeIds.Count} service types"
        );

        //var result = _mapper.Map<CreateServiceProviderProfileDto>(profile);
        var result = new CreateServiceProviderProfileDto
        {
            Name = profile.Name,
            Surname = profile.Surname,
            City = profile.City,
            Age = profile.Age.Value,
            GenderId = profile.GenderId.Value,
            ExperienceYears = profile.ExperienceYears.Value,
            ParentCategoryId = profile.ParentCategoryId.Value,
            ServiceTypeIds = request.Profile.ServiceTypeIds
        };
        return new ResponseModel<CreateServiceProviderProfileDto>
        {
            Data = result,
            IsSuccess = true
        };
    }
}
