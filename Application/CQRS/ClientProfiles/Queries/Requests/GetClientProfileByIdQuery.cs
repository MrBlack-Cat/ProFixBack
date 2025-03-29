using Application.CQRS.ClientProfiles.DTOs;
using Common.GlobalResponse;
using MediatR;

namespace Application.CQRS.ClientProfiles.Queries.Requests;

public class GetClientProfileByIdQuery : IRequest<ResponseModel<GetClientProfileByIdDto>>
{
    public int Id { get; set; }

    public GetClientProfileByIdQuery(int id)
    {
        Id = id;
    }
}
