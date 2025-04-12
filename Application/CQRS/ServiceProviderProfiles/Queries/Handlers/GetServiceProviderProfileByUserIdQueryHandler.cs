using Application.CQRS.ServiceProviderProfiles.DTOs;
using Application.CQRS.ServiceProviderProfiles.Queries.Requests;
using AutoMapper;
using Common.Exceptions;
using Common.GlobalResponse;
using MediatR;
using Repository.Common;

namespace Application.CQRS.ServiceProviderProfiles.Queries.Handlers;

public class GetServiceProviderProfileByUserIdQueryHandler : IRequestHandler<GetServiceProviderProfileByUserIdQuery, ResponseModel<GetServiceProviderProfileByUserIdDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetServiceProviderProfileByUserIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseModel<GetServiceProviderProfileByUserIdDto>> Handle(GetServiceProviderProfileByUserIdQuery request, CancellationToken cancellationToken)
    {
        var profile = await _unitOfWork.ServiceProviderProfileRepository.GetByUserIdAsync(request.UserId);

        if (profile == null)
            throw new NotFoundException("Service provider profile not found");

        var dto = _mapper.Map<GetServiceProviderProfileByUserIdDto>(profile);

        return new ResponseModel<GetServiceProviderProfileByUserIdDto>
        {
            Data = dto,
            IsSuccess = true
        };
    }
}
