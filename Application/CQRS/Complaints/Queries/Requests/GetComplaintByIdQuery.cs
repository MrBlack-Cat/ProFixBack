using Common.GlobalResponse;
using MediatR;
using Application.CQRS.Complaints.DTOs;

namespace Application.CQRS.Complaints.Queries.Requests;

public class GetComplaintByIdQuery : IRequest<ResponseModel<GetComplaintByIdDto>>
{
    public int Id { get; set; }

    public GetComplaintByIdQuery(int id)
    {
        Id = id;
    }
}
