using Application.CQRS.Certificates.DTOs;
using Common.GlobalResponse;
using MediatR;

namespace Application.CQRS.Certificates.Queries.Requests;

public record GetAllCertificatesQuery()
    : IRequest<ResponseModel<List<CertificateListDto>>>;
