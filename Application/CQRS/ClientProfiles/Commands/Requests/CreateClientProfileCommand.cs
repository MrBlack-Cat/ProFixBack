using Application.CQRS.ClientProfiles.DTOs;
using Common.GlobalResponse;
using MediatR;

public record CreateClientProfileCommand : IRequest<ResponseModel<CreateClientProfileDto>>
{
    public CreateClientProfileDto Profile { get; set; }

    public CreateClientProfileCommand(CreateClientProfileDto profile)
    {
        Profile = profile;
    }
}
    