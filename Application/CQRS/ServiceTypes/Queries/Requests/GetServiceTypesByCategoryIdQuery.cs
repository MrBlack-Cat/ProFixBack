using Application.CQRS.ServiceTypes.DTOs;
using Common.GlobalResponse;
using MediatR;


public record GetServiceTypesByCategoryIdQuery(int ParentCategoryId) : IRequest<ResponseModel<List<ServiceTypeListDto>>>;
