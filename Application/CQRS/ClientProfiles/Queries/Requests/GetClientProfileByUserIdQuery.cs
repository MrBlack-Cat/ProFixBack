using Application.CQRS.ClientProfiles.DTOs;
using Common.GlobalResponse;
using MediatR;

namespace Application.CQRS.ClientProfiles.Queries.Requests;

public record GetClientProfileByUserIdQuery(int UserId) : IRequest<ResponseModel<GetClientProfileByIdDto>>;
