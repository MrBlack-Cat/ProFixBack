using Application.CQRS.ServiceProviderProfiles.DTOs;
using Application.CQRS.ServiceProviderProfiles.Queries.Requests;
using AutoMapper;
using Common.Exceptions;
using Common.GlobalResponse;
using Domain.Entities;
using MediatR;
using Repository.Common;

namespace Application.CQRS.ServiceProviderProfiles.Queries.Handlers;

public class GetServiceProviderProfileByIdQueryHandler : IRequestHandler<GetServiceProviderProfileByIdQuery, ResponseModel<GetServiceProviderProfileByIdDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetServiceProviderProfileByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseModel<GetServiceProviderProfileByIdDto>> Handle(GetServiceProviderProfileByIdQuery request, CancellationToken cancellationToken)
    {
        var profile = await _unitOfWork.ServiceProviderProfileRepository.GetByIdAsync(request.Id);
        if (profile == null)
            throw new NotFoundException("Service provider profile not found");

        var result = _mapper.Map<GetServiceProviderProfileByIdDto>(profile);
        return new ResponseModel<GetServiceProviderProfileByIdDto> { Data = result, IsSuccess = true };
    }
}

