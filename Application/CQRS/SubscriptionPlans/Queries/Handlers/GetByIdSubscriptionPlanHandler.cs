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

public class GetSubscriptionPlanByIdHandler(IUnitOfWork unitOfWork, IMapper mapper , IActivityLoggerService activityLogger , IUserContext userContext) : IRequestHandler<GetSubscriptionPlanByIdRequest, ResponseModel<GetSubscriptionPlanByIdDto>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly IActivityLoggerService _activityLogger = activityLogger;
    private readonly IUserContext _userContext = userContext;

    public async Task<ResponseModel<GetSubscriptionPlanByIdDto>> Handle(GetSubscriptionPlanByIdRequest request, CancellationToken cancellationToken)
    {
        var subscriptionPlanEntity = await _unitOfWork.SubscriptionPlanRepository.GetByIdAsync(request.Id);
        if (subscriptionPlanEntity == null)
        {
            return new ResponseModel<GetSubscriptionPlanByIdDto>
            {
                IsSuccess = false,
                Errors = ["Subscription plan not found."]
            };
        }


        var currentUserId = _userContext.GetCurrentUserId();

        // Log yazırıq
        await _activityLogger.LogAsync(
            userId: currentUserId.Value,
            action: "View",
            entityType: "SubscriptionPlan",
            entityId: subscriptionPlanEntity.Id,
            performedBy: currentUserId,
            description: $"User {currentUserId.Value} (ID: {currentUserId}) viewed Subscription Plan: {subscriptionPlanEntity.PlanName} (ID: {subscriptionPlanEntity.Id})."
        );


        var responseDto = _mapper.Map<GetSubscriptionPlanByIdDto>(subscriptionPlanEntity);

        return new ResponseModel<GetSubscriptionPlanByIdDto>
        {
            Data = responseDto,
            IsSuccess = true
        };
    }
}
