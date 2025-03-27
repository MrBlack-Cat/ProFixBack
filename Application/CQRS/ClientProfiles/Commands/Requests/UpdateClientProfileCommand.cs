using Application.CQRS.ClientProfiles.DTOs;
using Common.GlobalResponse;
using MediatR;

namespace Application.CQRS.ClientProfiles.Commands.Requests;

public record UpdateClientProfileCommand(
    int Id,
    int UpdatedBy,
    UpdateClientProfileDto Dto
) : IRequest<ResponseModel<UpdateClientProfileDto>>;

