using Application.CQRS.ServiceTypes.DTOs;
using Common.GlobalResponse;
using MediatR;

namespace Application.CQRS.ServiceTypes.Queries.Requests;

public record GetServiceTypesByCategoryNameQuery(string CategoryName)
    : IRequest<ResponseModel<List<ServiceTypeListDto>>>;
