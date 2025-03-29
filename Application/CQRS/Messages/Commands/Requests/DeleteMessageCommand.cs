using Common.GlobalResponse;
using MediatR;

namespace Application.CQRS.Messages.Commands.Requests;

public record DeleteMessageCommand(int Id, int DeletedByUserId, string Reason)
    : IRequest<ResponseModel<string>>;
