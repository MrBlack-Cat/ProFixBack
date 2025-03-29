using Application.Common.Interfaces;
using Application.CQRS.ServiceProviderProfiles.DTOs;
using AutoMapper;
using Common.Exceptions;
using Common.GlobalResponse;
using Domain.Entities;
using MediatR;
using Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.ServiceProviderProfiles.Handler;

public class CreateServiceProviderProfileHandler
{


    public record struct Command(CreateServiceProviderProfileDto serviceProviderProfileDto) : IRequest<ResponseModel<CreateServiceProviderProfileDto>>;



    public sealed class Handler(IUnitOfWork unitOfWork, IMapper mapper, IActivityLoggerService activityLogger) : IRequestHandler<Command, ResponseModel<CreateServiceProviderProfileDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IActivityLoggerService _activityLogger = activityLogger;



        public async Task<ResponseModel<CreateServiceProviderProfileDto>> Handle(Command request, CancellationToken cancellationToken)
        {
            if (request.serviceProviderProfileDto == null)
                throw new BadRequestException("Invalid data provided.");

            var serviceProviderProfile = _mapper.Map<ServiceProviderProfile>(request.serviceProviderProfileDto);

            //db ye elave 
            await _unitOfWork.ServiceProviderProfileRepository.AddAsync(serviceProviderProfile);
            await _unitOfWork.SaveChangesAsync();


            #region ActivityLog
            await _activityLogger.LogAsync(
                userId: serviceProviderProfile.UserId,  
                action: "Create",  
                entityType: "ServiceProviderProfile",  
                entityId: serviceProviderProfile.Id,  
                performedBy: serviceProviderProfile.UserId,  
                description: $"Service provider profile created for {serviceProviderProfile.Name} in {serviceProviderProfile.City}."  
            );
            #endregion




            var responseDto = _mapper.Map<CreateServiceProviderProfileDto>(serviceProviderProfile);

            return new ResponseModel<CreateServiceProviderProfileDto>
            {
                Data = responseDto,
                IsSuccess = true,
                Errors = [],
            };
        }
    }
}
