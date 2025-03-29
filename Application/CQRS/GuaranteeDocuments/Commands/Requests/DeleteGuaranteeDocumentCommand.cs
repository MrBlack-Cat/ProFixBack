using Common.GlobalResponse;
using MediatR;

namespace Application.CQRS.GuaranteeDocuments.Commands.Requests;

public record DeleteGuaranteeDocumentCommand(int Id, int DeletedByUserId, string Reason)
    : IRequest<ResponseModel<string>>;
