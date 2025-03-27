using Application.CQRS.ServiceProviderProfiles.DTOs;
using AutoMapper;
using Common.GlobalResponse;
using MediatR;
using Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.ServiceProviderProfiles.Handler;

public class ServiceProviderProfileListHandler
{
    public record struct GetServiceProviderProfileListQuery() : IRequest<ResponseModel<List<ServiceProviderProfileListDto>>>;


    public sealed class ListHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetServiceProviderProfileListQuery, ResponseModel<List<ServiceProviderProfileListDto>>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<ResponseModel<List<ServiceProviderProfileListDto>>> Handle(GetServiceProviderProfileListQuery request, CancellationToken cancellationToken)
        {
            var profiles = await _unitOfWork.ServiceProviderProfileRepository.GetAllAsync();

            // AutoMapper ilə DTO-ya çeviririk
            var responseDtoList = _mapper.Map<List<ServiceProviderProfileListDto>>(profiles);

            return new ResponseModel<List<ServiceProviderProfileListDto>>
            {
                Data = responseDtoList,
                IsSuccess = true,
                Errors = ["Service provider profiles retrieved successfully."]
            };
        }
    }
}