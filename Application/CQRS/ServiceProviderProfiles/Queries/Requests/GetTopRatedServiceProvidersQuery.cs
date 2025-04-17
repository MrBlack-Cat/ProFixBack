using Common.GlobalResponse;
using MediatR;
using Application.CQRS.ServiceProviderProfiles.DTOs;

namespace Application.CQRS.ServiceProviderProfiles.Queries.Requests;

public class GetTopRatedServiceProvidersQuery : IRequest<ResponseModel<List<ServiceProviderProfileTopDto>>> { }
