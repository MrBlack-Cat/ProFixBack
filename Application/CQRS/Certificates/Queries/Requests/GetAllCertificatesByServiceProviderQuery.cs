using Application.CQRS.Certificates.DTOs;
using Common.GlobalResponse;
using MediatR;

namespace Application.CQRS.Certificates.Queries.Requests;

public record GetAllCertificatesByServiceProviderQuery(int ServiceProviderProfileId)
    : IRequest<ResponseModel<List<CertificateListDto>>>;
