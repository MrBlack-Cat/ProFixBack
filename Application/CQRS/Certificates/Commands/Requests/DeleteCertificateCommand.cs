using Common.GlobalResponse;
using MediatR;

namespace Application.CQRS.Certificates.Commands.Requests;

public record DeleteCertificateCommand(int Id, int DeletedByUserId, string Reason)
    : IRequest<ResponseModel<string>>;
