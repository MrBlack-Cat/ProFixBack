using Application.CQRS.ServiceProviderProfiles.DTOs;
using Common.GlobalResponse;
using MediatR;

namespace Application.CQRS.ServiceProviderProfiles.Queries.Requests
{
    public class GetServiceProviderProfileByUserIdQuery
        : IRequest<ResponseModel<GetServiceProviderProfileByUserIdDto>>
    {
        public int UserId { get; set; }

        public GetServiceProviderProfileByUserIdQuery(int userId)
        {
            UserId = userId;
        }
    }
}
