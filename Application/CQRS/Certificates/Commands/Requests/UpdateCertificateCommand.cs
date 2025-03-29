using Application.CQRS.Certificates.DTOs;
using Common.GlobalResponse;
using MediatR;

namespace Application.CQRS.Certificates.Commands.Requests;

public record UpdateCertificateCommand(int Id, int UpdatedByUserId, UpdateCertificateDto Dto)
    : IRequest<ResponseModel<UpdateCertificateDto>>;
