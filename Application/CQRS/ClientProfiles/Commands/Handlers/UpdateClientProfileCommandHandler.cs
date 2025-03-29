using Application.CQRS.ClientProfiles.Commands.Requests;
using Application.CQRS.ClientProfiles.DTOs;
using Application.Common.Interfaces;
using AutoMapper;
using Common.Exceptions;
using Common.GlobalResponse;
using Domain.Entities;
using MediatR;
using Repository.Common;

namespace Application.CQRS.ClientProfiles.Commands.Handlers;

public class UpdateClientProfileCommandHandler : IRequestHandler<UpdateClientProfileCommand, ResponseModel<UpdateClientProfileDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IAuthorizationService _authorizationService;
    private readonly IUserContext _userContext;
    private readonly IActivityLoggerService _activityLogger;

    public UpdateClientProfileCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IAuthorizationService authorizationService,
        IUserContext userContext,
        IActivityLoggerService activityLogger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _authorizationService = authorizationService;
        _userContext = userContext;
        _activityLogger = activityLogger;
    }

    public async Task<ResponseModel<UpdateClientProfileDto>> Handle(UpdateClientProfileCommand request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.ClientProfileRepository.GetByIdAsync(request.Id);
        if (entity is null || entity.IsDeleted)
            throw new NotFoundException("Client profile not found.");

        _authorizationService.AuthorizeOwnerOrAdmin(
            resourceOwnerId: entity.UserId,
            currentUserId: request.UpdatedBy,
            currentUserRole: _userContext.GetUserRole()
        );

        entity.Name = request.Dto.Name;
        entity.Surname = request.Dto.Surname;
        entity.City = request.Dto.City;
        entity.AvatarUrl = request.Dto.AvatarUrl;
        entity.About = request.Dto.About;
        entity.OtherContactLinks = request.Dto.OtherContactLinks;
        entity.UpdatedAt = DateTime.UtcNow;
        entity.UpdatedBy = request.UpdatedBy;

        await _unitOfWork.ClientProfileRepository.UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync();

        await _activityLogger.LogAsync(
            userId: entity.UserId,
            action: "Update",
            entityType: "ClientProfile",
            entityId: entity.Id,
            performedBy: request.UpdatedBy
        );

        var resultDto = _mapper.Map<UpdateClientProfileDto>(entity);
        return new ResponseModel<UpdateClientProfileDto> { Data = resultDto, IsSuccess = true };
    }
}
