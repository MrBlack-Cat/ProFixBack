using Application.Common.Interfaces;
using Application.CQRS.ClientProfiles.Commands.Requests;
using Application.CQRS.ClientProfiles.DTOs;
using AutoMapper;
using Common.Exceptions;
using Common.GlobalResponse;
using Domain.Entities;
using MediatR;
using Repository.Common;

namespace Application.CQRS.ClientProfiles.Commands.Handlers;

public class CreateClientProfileCommandHandler : IRequestHandler<CreateClientProfileCommand, ResponseModel<CreateClientProfileDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IActivityLoggerService _activityLogger;


    public CreateClientProfileCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IActivityLoggerService activityLogger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _activityLogger = activityLogger;


    }

    public async Task<ResponseModel<CreateClientProfileDto>> Handle(CreateClientProfileCommand request, CancellationToken cancellationToken)
    {
        var existing = await _unitOfWork.ClientProfileRepository.GetByUserIdAsync(request.Profile.UserId);
        if (existing is not null)
            throw new ConflictException("Client profile already exists for this user.");

        var profile = new ClientProfile
        {
            UserId = request.Profile.UserId,
            Name = request.Profile.Name,
            Surname = request.Profile.Surname,
            City = request.Profile.City,
            AvatarUrl = request.Profile.AvatarUrl,
            About = request.Profile.About,
            OtherContactLinks = request.Profile.OtherContactLinks,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.ClientProfileRepository.AddAsync(profile);
        await _unitOfWork.SaveChangesAsync();

        await _activityLogger.LogAsync(
        userId: request.Profile.UserId,
        action: "Create",
        entityType: "ClientProfile",
        entityId: profile.Id);


        var result = _mapper.Map<CreateClientProfileDto>(profile);
        return new ResponseModel<CreateClientProfileDto> { Data = result, IsSuccess = true };
    }
}
