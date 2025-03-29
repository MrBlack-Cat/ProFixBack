using Common.GlobalResponse;
using MediatR;

namespace Application.CQRS.ClientProfiles.Commands.Requests;

public record DeleteClientProfileCommand(int Id, int DeletedBy, string DeletedReason)
    : IRequest<ResponseModel<string>>;
