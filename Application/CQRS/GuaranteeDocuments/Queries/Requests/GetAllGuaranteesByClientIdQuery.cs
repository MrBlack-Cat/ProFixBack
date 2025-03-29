using Application.CQRS.GuaranteeDocuments.DTOs;
using Common.GlobalResponse;
using MediatR;

namespace Application.CQRS.GuaranteeDocuments.Queries.Requests;

public record GetAllGuaranteesByClientIdQuery(int ClientProfileId)
    : IRequest<ResponseModel<List<GuaranteeDocumentListDto>>>;
