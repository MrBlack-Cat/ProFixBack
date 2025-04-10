using Common.GlobalResponse;
using Domain.Entities;
using MediatR;

public record GetClientProfileByCurrentUserQuery(int UserId) : IRequest<ResponseModel<ClientProfile>>;
