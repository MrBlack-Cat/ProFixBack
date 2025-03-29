using Application.CQRS.Certificates.DTOs;
using Common.GlobalResponse;
using MediatR;

namespace Application.CQRS.Certificates.Commands.Requests;

public record CreateCertificateCommand(CreateCertificateDto Dto, int CreatedByUserId)
    : IRequest<ResponseModel<CreateCertificateDto>>;
