using Application.CQRS.GuaranteeDocuments.DTOs;
using Common.GlobalResponse;
using MediatR;

namespace Application.CQRS.GuaranteeDocuments.Commands.Requests;

public record UpdateGuaranteeDocumentCommand(int Id, int UpdatedByUserId, UpdateGuaranteeDocumentDto Dto)
    : IRequest<ResponseModel<UpdateGuaranteeDocumentDto>>;
