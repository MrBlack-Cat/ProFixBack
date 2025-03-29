using Application.CQRS.Certificates.DTOs;
using Common.GlobalResponse;
using MediatR;

namespace Application.CQRS.Certificates.Queries.Requests;

public record GetCertificateByIdQuery(int Id)
    : IRequest<ResponseModel<GetCertificateByIdDto>>;
