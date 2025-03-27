using Application.CQRS.Certificates.Commands.Requests;
using Application.CQRS.Certificates.DTOs;
using AutoMapper;
using Common.GlobalResponse;
using Domain.Entities;
using MediatR;
using Repository.Common;
using Application.Common.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Common.Exceptions;

namespace Application.CQRS.Certificates.Commands.Handlers;

public class CreateCertificateCommandHandler : IRequestHandler<CreateCertificateCommand, ResponseModel<CreateCertificateDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IActivityLoggerService _activityLogger;

    public CreateCertificateCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IActivityLoggerService activityLogger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _activityLogger = activityLogger;
    }

    public async Task<ResponseModel<CreateCertificateDto>> Handle(CreateCertificateCommand request, CancellationToken cancellationToken)
    {
        var userId = request.CreatedByUserId;

        var profile = await _unitOfWork.ServiceProviderProfileRepository.GetByUserIdAsync(userId);
        if (profile is null)
            throw new NotFoundException("ServiceProviderProfile not found for current user.");

        var dto = request.Dto;

        var certificate = new Certificate
        {
            ServiceProviderProfileId = profile.Id,
            Title = dto.Title,
            Description = dto.Description,
            FileUrl = dto.FileUrl,
            IssuedAt = dto.IssuedAt,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = request.CreatedByUserId
        };

        await _unitOfWork.CertificateRepository.AddAsync(certificate);
        await _unitOfWork.SaveChangesAsync();

        await _activityLogger.LogAsync(
            userId: request.CreatedByUserId,
            action: "Create",
            entityType: "Certificate",
            entityId: certificate.Id,
            performedBy: request.CreatedByUserId
        );

        var result = _mapper.Map<CreateCertificateDto>(certificate);
        return new ResponseModel<CreateCertificateDto> { Data = result, IsSuccess = true };
    }
}
