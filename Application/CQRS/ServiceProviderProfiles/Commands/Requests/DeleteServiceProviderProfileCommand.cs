using Common.GlobalResponse;
using MediatR;

namespace Application.CQRS.ServiceProviderProfiles.Commands.Requests;

public record DeleteServiceProviderProfileCommand(int Id, int DeletedBy, string DeletedReason)
    : IRequest<ResponseModel<string>>;
