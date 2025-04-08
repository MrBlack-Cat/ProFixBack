using Application.CQRS.ServiceProviderProfiles.DTOs;
using Common.GlobalResponse;
using MediatR;

namespace Application.CQRS.ServiceProviderProfiles.Queries.Requests;

public record GetServiceProviderProfileByIdQuery(int Id) : IRequest<ResponseModel<GetServiceProviderProfileByIdDto>>;
