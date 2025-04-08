using Application.CQRS.ClientProfiles.DTOs;
using Common.GlobalResponse;
using MediatR;

public record CreateClientProfileCommand(CreateClientProfileDto Profile, int UserId)
    : IRequest<ResponseModel<CreateClientProfileDto>>;
