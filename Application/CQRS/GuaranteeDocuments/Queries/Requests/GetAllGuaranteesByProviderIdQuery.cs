using Application.CQRS.GuaranteeDocuments.DTOs;
using Common.GlobalResponse;
using MediatR;

namespace Application.CQRS.GuaranteeDocuments.Queries.Requests;

public record GetAllGuaranteesByProviderIdQuery(int ServiceProviderProfileId)
    : IRequest<ResponseModel<List<GuaranteeDocumentListDto>>>;
