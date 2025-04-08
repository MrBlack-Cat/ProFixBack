using Application.CQRS.ServiceTypes.DTOs;
using Application.CQRS.ServiceTypes.Queries.Requests;
using AutoMapper;
using Common.GlobalResponse;
using MediatR;
using Repository.Common;

public class GetServiceTypesByNameQueryHandler : IRequestHandler<GetServiceTypesByNameQuery, ResponseModel<List<ServiceTypeListDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetServiceTypesByNameQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseModel<List<ServiceTypeListDto>>> Handle(GetServiceTypesByNameQuery request, CancellationToken cancellationToken)
    {
        var result = await _unitOfWork.ServiceTypeRepository.SearchByNameAsync(request.Name);
        var dto = _mapper.Map<List<ServiceTypeListDto>>(result);

        return new ResponseModel<List<ServiceTypeListDto>> { Data = dto, IsSuccess = true };
    }
}
