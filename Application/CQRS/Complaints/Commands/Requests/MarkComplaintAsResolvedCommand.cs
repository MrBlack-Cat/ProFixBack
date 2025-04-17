using Common.GlobalResponse;
using MediatR;

namespace Application.CQRS.Complaints.Commands.Requests;

public class MarkComplaintAsResolvedCommand : IRequest<ResponseModel<bool>>
{
    public int ComplaintId { get; set; }

    public MarkComplaintAsResolvedCommand(int complaintId)
    {
        ComplaintId = complaintId;
    }
}
