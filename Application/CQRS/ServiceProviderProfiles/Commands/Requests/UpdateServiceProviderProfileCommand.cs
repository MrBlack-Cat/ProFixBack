using Application.CQRS.ServiceProviderProfiles.DTOs;
using Common.GlobalResponse;
using MediatR;

namespace Application.CQRS.ServiceProviderProfiles.Commands.Requests;

public record UpdateServiceProviderProfileCommand(
    int Id,
    int UpdatedBy,
    UpdateServiceProviderProfileDto Profile
) : IRequest<ResponseModel<UpdateServiceProviderProfileDto>>;
