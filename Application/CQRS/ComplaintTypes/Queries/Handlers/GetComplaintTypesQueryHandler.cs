using Application.CQRS.ComplaintTypes.DTOs;
using Application.CQRS.ComplaintTypes.Queries.Requests;
using Common.GlobalResponse;
using MediatR;
using Repository.Common;

namespace Application.CQRS.ComplaintTypes.Queries.Handlers;

public class GetComplaintTypesQueryHandler : IRequestHandler<GetComplaintTypesQuery, ResponseModel<List<ComplaintTypeDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetComplaintTypesQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseModel<List<ComplaintTypeDto>>> Handle(GetComplaintTypesQuery request, CancellationToken cancellationToken)
    {
        var types = await _unitOfWork.ComplaintTypeRepository.GetAllAsync();
        var dto = types.Select(t => new ComplaintTypeDto
        {
            Id = t.Id,
            Name = t.Name
        }).ToList();

        return ResponseModel<List<ComplaintTypeDto>>.Success(dto);
    }
}
