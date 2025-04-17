using Application.CQRS.Complaints.Commands.Requests;
using Common.Exceptions;
using Common.GlobalResponse;
using MediatR;
using Repository.Common;

namespace Application.CQRS.Complaints.Commands.Handlers;

public class MarkComplaintAsViewedCommandHandler : IRequestHandler<MarkComplaintAsViewedCommand, ResponseModel<bool>>
{
    private readonly IUnitOfWork _unitOfWork;

    public MarkComplaintAsViewedCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseModel<bool>> Handle(MarkComplaintAsViewedCommand request, CancellationToken cancellationToken)
    {
        var complaint = await _unitOfWork.ComplaintRepository.GetByIdAsync(request.ComplaintId);
        if (complaint == null)
            throw new NotFoundException("Complaint not found");

        complaint.IsViewed = true;
        await _unitOfWork.ComplaintRepository.UpdateAsync(complaint);
        await _unitOfWork.SaveChangesAsync();

        return ResponseModel<bool>.Success(true);
    }
}
