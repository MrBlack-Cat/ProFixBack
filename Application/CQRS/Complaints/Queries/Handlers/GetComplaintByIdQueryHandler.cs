using AutoMapper;
using Common.Exceptions;
using Common.GlobalResponse;
using Domain.Entities;
using MediatR;
using Repository.Common;
using Application.CQRS.Complaints.Queries.Requests;
using Application.CQRS.Complaints.DTOs;

namespace Application.CQRS.Complaints.Queries.Handlers;

public class GetComplaintByIdQueryHandler : IRequestHandler<GetComplaintByIdQuery, ResponseModel<GetComplaintByIdDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetComplaintByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseModel<GetComplaintByIdDto>> Handle(GetComplaintByIdQuery request, CancellationToken cancellationToken)
    {
        var complaint = await _unitOfWork.ComplaintRepository.GetByIdAsync(request.Id);
        if (complaint == null)
            throw new NotFoundException("Complaint not found");

        var result = _mapper.Map<GetComplaintByIdDto>(complaint);
        return ResponseModel<GetComplaintByIdDto>.Success(result);
    }
}
