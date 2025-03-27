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

public class UpdateGuaranteeDocumentCommandHandler : IRequestHandler<UpdateGuaranteeDocumentCommand, ResponseModel<UpdateGuaranteeDocumentDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IAuthorizationService _authorizationService;
    private readonly IUserContext _userContext;
    private readonly IActivityLoggerService _activityLogger;

    public UpdateGuaranteeDocumentCommandHandler(
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

    public async Task<ResponseModel<UpdateGuaranteeDocumentDto>> Handle(UpdateGuaranteeDocumentCommand request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.GuaranteeDocumentRepository.GetByIdAsync(request.Id);
        if (entity is null || entity.IsDeleted)
            throw new NotFoundException("Guarantee document not found.");

        if (entity.CreatedBy == null)
            throw new ForbiddenException("Unable to verify owner of this document.");

        _authorizationService.AuthorizeOwnerOrAdmin(
            resourceOwnerId: entity.CreatedBy.Value,
            currentUserId: request.UpdatedByUserId,
            currentUserRole: _userContext.GetUserRole()
        );

        entity.Title = request.Dto.Title;
        entity.Description = request.Dto.Description;
        entity.FileUrl = request.Dto.FileUrl;
        entity.UpdatedAt = DateTime.UtcNow;
        entity.UpdatedBy = request.UpdatedByUserId;

        await _unitOfWork.GuaranteeDocumentRepository.UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync();

        await _activityLogger.LogAsync(
            userId: entity.CreatedBy.Value,
            action: "Update",
            entityType: "GuaranteeDocument",
            entityId: entity.Id,
            performedBy: request.UpdatedByUserId
        );

        var resultDto = _mapper.Map<UpdateGuaranteeDocumentDto>(entity);
        return new ResponseModel<UpdateGuaranteeDocumentDto> { Data = resultDto, IsSuccess = true };
    }
}
