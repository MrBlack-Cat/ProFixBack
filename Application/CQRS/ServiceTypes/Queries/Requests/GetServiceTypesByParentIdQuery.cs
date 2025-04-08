using Application.CQRS.ServiceTypes.DTOs;
using Common.GlobalResponse;
using MediatR;

namespace Application.CQRS.ServiceTypes.Queries.Requests
{
    public class GetServiceTypesByParentIdQuery : IRequest<ResponseModel<List<ServiceTypeListDto>>>
    {
        public int ParentCategoryId { get; set; }

        public GetServiceTypesByParentIdQuery(int parentCategoryId)
        {
            ParentCategoryId = parentCategoryId;
        }
    }
}
