using AutoMapper;
using Common.GlobalResponse;
using Domain.Entities;
using MediatR;
using Repository.Common;
using Application.CQRS.Complaints.Queries.Requests;
using Application.CQRS.Complaints.DTOs;

namespace Application.CQRS.Complaints.Queries.Handlers;

public class GetComplaintListQueryHandler : IRequestHandler<GetComplaintListQuery, ResponseModel<List<ComplaintListDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetComplaintListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseModel<List<ComplaintListDto>>> Handle(GetComplaintListQuery request, CancellationToken cancellationToken)
    {
        var complaints = await _unitOfWork.ComplaintRepository.GetAllAsync();
        var result = _mapper.Map<List<ComplaintListDto>>(complaints);
        return ResponseModel<List<ComplaintListDto>>.Success(result);
    }
}
