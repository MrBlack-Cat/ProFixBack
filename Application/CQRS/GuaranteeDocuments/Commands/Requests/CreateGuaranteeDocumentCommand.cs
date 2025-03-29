using Application.CQRS.GuaranteeDocuments.DTOs;
using Common.GlobalResponse;
using MediatR;

namespace Application.CQRS.GuaranteeDocuments.Commands.Requests;

public record CreateGuaranteeDocumentCommand(CreateGuaranteeDocumentDto Dto, int CreatedByUserId)
    : IRequest<ResponseModel<CreateGuaranteeDocumentDto>>;
