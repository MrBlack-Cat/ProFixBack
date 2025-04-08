using Application.CQRS.ServiceProviderProfiles.DTOs;
using Application.CQRS.ServiceProviderProfiles.Queries.Requests;
using AutoMapper;
using Common.GlobalResponse;
using MediatR;
using Repository.Common;

namespace Application.CQRS.ServiceProviderProfiles.Queries.Handlers;

public class GetAllServiceProviderProfilesQueryHandler : IRequestHandler<GetAllServiceProviderProfilesQuery, ResponseModel<List<ServiceProviderProfileListDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllServiceProviderProfilesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseModel<List<ServiceProviderProfileListDto>>> Handle(GetAllServiceProviderProfilesQuery request, CancellationToken cancellationToken)
    {
        var profiles = await _unitOfWork.ServiceProviderProfileRepository.GetAllAsync();
        var result = _mapper.Map<List<ServiceProviderProfileListDto>>(profiles);

        return new ResponseModel<List<ServiceProviderProfileListDto>> { Data = result, IsSuccess = true };
    }
}
