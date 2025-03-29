using Application.Common.Interfaces;
using Application.CQRS.SubscriptionPlans.DTOs;
using Application.CQRS.SubscriptionPlans.Queries.Requests;
using AutoMapper;
using Common.GlobalResponse;
using MediatR;
using Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.SubscriptionPlans.Queries.Handlers;
public class GetListSubscriptionPlanHandler(IUnitOfWork unitOfWork, IMapper mapper , IActivityLoggerService activityLogger , IUserContext userContext) : IRequestHandler<GetListSubscriptionPlanRequest, ResponseModel<List<SubscriptionPlanListDto>>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly IActivityLoggerService _activityLogger = activityLogger;
    private readonly IUserContext _userContext = userContext;

    public async Task<ResponseModel<List<SubscriptionPlanListDto>>> Handle(GetListSubscriptionPlanRequest request, CancellationToken cancellationToken)
    {
        var subscriptionPlans = await _unitOfWork.SubscriptionPlanRepository.GetAllAsync();

        if (subscriptionPlans == null || subscriptionPlans.Count() == 0) //burada count a () elave eledim cunki null olarsa exception atir
        {
            return new ResponseModel<List<SubscriptionPlanListDto>>
            {
                IsSuccess = false,
                Errors =[ "No subscription plans found."]
            };
        }

        #region ActivityLog

        var currentUserId = _userContext.GetCurrentUserId(); 
        var allPlanIds = subscriptionPlans.Select(x => x.Id).ToList();

        await _activityLogger.LogAsync(
              userId: currentUserId.Value,
              action: "View",
              entityType: "SubscriptionPlan",
              entityIds: allPlanIds,
              entityId: 0,  
              performedBy: currentUserId,
              description: $"User {currentUserId.Value} (ID: {currentUserId}) viewed all Subscription Plans."
          );

        #endregion


        var responseDto = _mapper.Map<List<SubscriptionPlanListDto>>(subscriptionPlans);

        return new ResponseModel<List<SubscriptionPlanListDto>>
        {
            Data = responseDto,
            IsSuccess = true
        };
    }
}