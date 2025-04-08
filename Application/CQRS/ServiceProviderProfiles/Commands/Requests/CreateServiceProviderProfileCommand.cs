using Application.CQRS.ServiceProviderProfiles.DTOs;
using Common.GlobalResponse;
using MediatR;

namespace Application.CQRS.ServiceProviderProfiles.Commands.Requests;

public record CreateServiceProviderProfileCommand(CreateServiceProviderProfileDto Profile, int UserId)
    : IRequest<ResponseModel<CreateServiceProviderProfileDto>>;
