using Application.CQRS.ParentCategories.DTOs;
using Application.CQRS.ServiceTypes.DTOs;
using Common.GlobalResponse;
using MediatR;

namespace Application.CQRS.ServiceTypes.Queries.Requests;

public class GetParentCategoryByIdQuery : IRequest<ResponseModel<ParentCategoryListDto>>
{
    public int Id { get; set; }

    public GetParentCategoryByIdQuery(int id)
    {
        Id = id;
    }
}
