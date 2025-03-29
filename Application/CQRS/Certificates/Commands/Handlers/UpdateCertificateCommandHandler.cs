using Application.CQRS.Certificates.Commands.Requests;
using Application.CQRS.Certificates.DTOs;
using Application.Common.Interfaces;
using AutoMapper;
using Common.Exceptions;
using Common.GlobalResponse;
using Domain.Entities;
using MediatR;
using Repository.Common;

namespace Application.CQRS.Certificates.Commands.Handlers;

public class UpdateCertificateCommandHandler : IRequestHandler<UpdateCertificateCommand, ResponseModel<UpdateCertificateDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IAuthorizationService _authorizationService;
    private readonly IUserContext _userContext;
    private readonly IActivityLoggerService _activityLogger;

    public UpdateCertificateCommandHandler(
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

    public async Task<ResponseModel<UpdateCertificateDto>> Handle(UpdateCertificateCommand request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.CertificateRepository.GetByIdAsync(request.Id);
        if (entity is null || entity.IsDeleted)
            throw new NotFoundException("Certificate not found.");

        if (entity.CreatedBy == null)
            throw new ForbiddenException("Unable to verify owner of this certificate.");


        _authorizationService.AuthorizeOwnerOrAdmin(
            resourceOwnerId: entity.CreatedBy.Value,
            currentUserId: request.UpdatedByUserId,
            currentUserRole: _userContext.GetUserRole()
        );


        entity.Title = request.Dto.Title;
        entity.Description = request.Dto.Description;
        entity.FileUrl = request.Dto.FileUrl;
        entity.IssuedAt = request.Dto.IssuedAt;
        entity.UpdatedAt = DateTime.UtcNow;
        entity.UpdatedBy = request.UpdatedByUserId;

        await _unitOfWork.CertificateRepository.UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync();

        await _activityLogger.LogAsync(
            userId: entity.CreatedBy ?? request.UpdatedByUserId,
            action: "Update",
            entityType: "Certificate",
            entityId: entity.Id,
            performedBy: request.UpdatedByUserId
        );

        var resultDto = _mapper.Map<UpdateCertificateDto>(entity);
        return new ResponseModel<UpdateCertificateDto> { Data = resultDto, IsSuccess = true };
    }
}
