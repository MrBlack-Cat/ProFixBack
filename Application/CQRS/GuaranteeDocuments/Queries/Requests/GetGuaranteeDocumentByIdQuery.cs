using Application.CQRS.GuaranteeDocuments.DTOs;
using Common.GlobalResponse;
using MediatR;

namespace Application.CQRS.GuaranteeDocuments.Queries.Requests;

public record GetGuaranteeDocumentByIdQuery(int Id)
    : IRequest<ResponseModel<GetGuaranteeDocumentByIdDto>>;
