using Common.GlobalResponse;
using MediatR;

namespace Application.CQRS.Complaints.Commands.Requests;

public class MarkComplaintAsViewedCommand : IRequest<ResponseModel<bool>>
{
    public int ComplaintId { get; set; }

    public MarkComplaintAsViewedCommand(int complaintId)
    {
        ComplaintId = complaintId;
    }
}
