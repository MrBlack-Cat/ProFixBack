using Application.CQRS.ServiceProviderProfiles.DTOs;
using Common.GlobalResponse;
using MediatR;

public record GetServiceProvidersByCategoryIdQuery(int CategoryId)
    : IRequest<ResponseModel<List<ServiceProviderProfileListDto>>>;
