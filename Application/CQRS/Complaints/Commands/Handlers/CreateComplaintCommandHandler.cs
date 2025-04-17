using Application.CQRS.Complaints.Commands.Requests;
using Common.GlobalResponse;
using Domain.Entities;
using MediatR;
using Repository.Common;

namespace Application.CQRS.Complaints.Commands.Handlers;

public class CreateComplaintCommandHandler : IRequestHandler<CreateComplaintCommand, ResponseModel<int>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateComplaintCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseModel<int>> Handle(CreateComplaintCommand request, CancellationToken cancellationToken)
    {
        var complaint = new Complaint
        {
            FromUserId = request.FromUserId,
            ToUserId = request.Dto.ToUserId,
            TypeId = request.Dto.TypeId,
            Description = request.Dto.Description,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = request.FromUserId,
        };

        await _unitOfWork.ComplaintRepository.AddAsync(complaint);
        await _unitOfWork.SaveChangesAsync();

        return ResponseModel<int>.Success(complaint.Id);
    }
}
