using Application.Common.Interfaces;
using Application.CQRS.ServiceProviderProfiles.DTOs;
using AutoMapper;
using Common.GlobalResponse;
using Infrastructure.Services;
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


    public sealed class ListHandler(IUnitOfWork unitOfWork, IMapper mapper, IActivityLoggerService activityLogger , IUserContext userContext) : IRequestHandler<GetServiceProviderProfileListQuery, ResponseModel<List<ServiceProviderProfileListDto>>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IActivityLoggerService _activityLogger = activityLogger;
        private readonly IUserContext _userContext = userContext;



        public async Task<ResponseModel<List<ServiceProviderProfileListDto>>> Handle(GetServiceProviderProfileListQuery request, CancellationToken cancellationToken)
        {
            var profiles = await _unitOfWork.ServiceProviderProfileRepository.GetAllAsync();

            // AutoMapper ilə DTO-ya çeviririk
            var responseDtoList = _mapper.Map<List<ServiceProviderProfileListDto>>(profiles);

            #region ActivityLog


            var currentUserId = _userContext.GetCurrentUserId;

            await _activityLogger.LogAsync(
                  userId: 0, 
                  action: "Retrieve",
                  entityType: "ServiceProviderProfile",
                  entityId: 0,
                  performedBy: 0,
                  description: $"User {currentUserId} retrieved the list of service provider profiles."
              );

            #endregion



            return new ResponseModel<List<ServiceProviderProfileListDto>>
            {
                Data = responseDtoList,
                IsSuccess = true,
                Errors = ["Service provider profiles retrieved successfully."]
            };
        }
    }
}