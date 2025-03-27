using Application.CQRS.ServiceProviderProfiles.DTOs;
using AutoMapper;
using Common.Exceptions;
using Common.GlobalResponse;
using Domain.Types;
using MediatR;
using Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.ServiceProviderProfiles.Handler;

public class GetServiceProviderProfileByIdHandler
{
    public record struct GetServiceProviderProfileByIdQuery (int Id) : IRequest<ResponseModel<GetServiceProviderProfileByIdDto>>;


    public sealed class Handler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetServiceProviderProfileByIdQuery, ResponseModel<GetServiceProviderProfileByIdDto>>
    {

        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;


        public async Task<ResponseModel<GetServiceProviderProfileByIdDto>> Handle(GetServiceProviderProfileByIdQuery request, CancellationToken cancellationToken)
        {

            var profile = await _unitOfWork.ServiceProviderProfileRepository.GetByIdAsync(request.Id);
            if (profile == null)
                throw new NotFoundException("Service Provider Profile not found.");

            
            var responseDto = _mapper.Map<GetServiceProviderProfileByIdDto>(profile);

            return new ResponseModel<GetServiceProviderProfileByIdDto>
            {
                Data = responseDto,
                IsSuccess = true,
                Errors = [],
            };
            
        }
    }

}
