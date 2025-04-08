using Application.CQRS.ServiceTypes.DTOs;
using Common.GlobalResponse;
using MediatR;

public record GetAllServiceTypesQuery() : IRequest<ResponseModel<List<ServiceTypeListDto>>>;

