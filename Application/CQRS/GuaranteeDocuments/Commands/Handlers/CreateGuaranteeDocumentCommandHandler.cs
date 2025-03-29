using Application.CQRS.GuaranteeDocuments.Commands.Requests;
using Application.CQRS.GuaranteeDocuments.DTOs;
using Application.Common.Interfaces;
using AutoMapper;
using Common.Exceptions;
using Common.GlobalResponse;
using Domain.Entities;
using MediatR;
using Repository.Common;

namespace Application.CQRS.GuaranteeDocuments.Commands.Handlers;

public class CreateGuaranteeDocumentCommandHandler : IRequestHandler<CreateGuaranteeDocumentCommand, ResponseModel<CreateGuaranteeDocumentDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IActivityLoggerService _activityLogger;
    private readonly IAuthorizationService _authorizationService;
    private readonly IUserContext _userContext;

    public CreateGuaranteeDocumentCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IActivityLoggerService activityLogger,
        IAuthorizationService authorizationService,
        IUserContext userContext)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _activityLogger = activityLogger;
        _authorizationService = authorizationService;
        _userContext = userContext;
    }

    public async Task<ResponseModel<CreateGuaranteeDocumentDto>> Handle(CreateGuaranteeDocumentCommand request, CancellationToken cancellationToken)
    {
        var userId = request.CreatedByUserId;
        var userRole = _userContext.GetUserRole();

        _authorizationService.AuthorizeRoles(userRole, "ServiceProvider", "Admin");

        var profile = await _unitOfWork.ServiceProviderProfileRepository.GetByUserIdAsync(userId);
        if (profile is null)
            throw new NotFoundException("ServiceProvider profile not found.");

        var dto = request.Dto;

        var entity = new GuaranteeDocument
        {
            ServiceProviderProfileId = profile.Id,
            ClientProfileId = dto.ClientProfileId,
            Title = dto.Title,
            Description = dto.Description,
            FileUrl = dto.FileUrl,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = userId
        };

        await _unitOfWork.GuaranteeDocumentRepository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();

        await _activityLogger.LogAsync(
            userId: userId,
            action: "Create",
            entityType: "GuaranteeDocument",
            entityId: entity.Id,
            performedBy: userId
        );

        var result = _mapper.Map<CreateGuaranteeDocumentDto>(entity);
        return new ResponseModel<CreateGuaranteeDocumentDto> { Data = result, IsSuccess = true };
    }
}
