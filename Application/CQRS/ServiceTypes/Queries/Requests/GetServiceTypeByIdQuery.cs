using Application.CQRS.ServiceTypes.DTOs;
using Common.GlobalResponse;
using MediatR;

namespace Application.CQRS.ServiceTypes.Queries.Requests;

public class GetServiceTypeByIdQuery : IRequest<ResponseModel<ServiceTypeListDto>>
{
    public int Id { get; set; }

    public GetServiceTypeByIdQuery(int id)
    {
        Id = id;
    }
}
