using Application.CQRS.Complaints.Commands.Requests;
using Common.Exceptions;
using Common.GlobalResponse;
using MediatR;
using Repository.Common;

namespace Application.CQRS.Complaints.Commands.Handlers;

public class MarkComplaintAsResolvedCommandHandler : IRequestHandler<MarkComplaintAsResolvedCommand, ResponseModel<bool>>
{
    private readonly IUnitOfWork _unitOfWork;

    public MarkComplaintAsResolvedCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseModel<bool>> Handle(MarkComplaintAsResolvedCommand request, CancellationToken cancellationToken)
    {
        var complaint = await _unitOfWork.ComplaintRepository.GetByIdAsync(request.ComplaintId);
        if (complaint == null)
            throw new NotFoundException("Complaint not found");

        complaint.IsResolved = true;
        await _unitOfWork.ComplaintRepository.UpdateAsync(complaint);
        await _unitOfWork.SaveChangesAsync();

        return ResponseModel<bool>.Success(true);
    }
}
