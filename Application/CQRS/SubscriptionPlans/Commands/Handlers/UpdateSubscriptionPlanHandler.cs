using Application.Common.Interfaces;
using Application.CQRS.SubscriptionPlans.Commands.Requests;
using Application.CQRS.SubscriptionPlans.DTOs;
using AutoMapper;
using Common.GlobalResponse;
using MediatR;
using Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.SubscriptionPlans.Commands.Handlers;

public class UpdateSubscriptionPlanHandler(IUnitOfWork unitOfWork, IMapper mapper , IActivityLoggerService activityLogger , IUserContext userContext ) : IRequestHandler<UpdateSubscriptionPlanRequest, ResponseModel<UpdateSubscriptionPlanDto>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly IActivityLoggerService _activityLogger = activityLogger;
    private readonly IUserContext _userContext = userContext;

    public async Task<ResponseModel<UpdateSubscriptionPlanDto>> Handle(UpdateSubscriptionPlanRequest request, CancellationToken cancellationToken)
    {
        // Subscription Planı tapırıq
        var subscriptionPlanEntity = await _unitOfWork.SubscriptionPlanRepository.GetByIdAsync(request.SubscriptionPlanDto.Id);
        if (subscriptionPlanEntity == null)
        {
            return new ResponseModel<UpdateSubscriptionPlanDto>
            {
                IsSuccess = false,
                Errors = ["Subscription plan not found."]
            };
        }

        subscriptionPlanEntity.PlanName = request.SubscriptionPlanDto.PlanName;
        subscriptionPlanEntity.Price = request.SubscriptionPlanDto.Price;
        subscriptionPlanEntity.DurationInDays = request.SubscriptionPlanDto.DurationInDays;
        subscriptionPlanEntity.StartDate = request.SubscriptionPlanDto.StartDate;
        subscriptionPlanEntity.EndDate = request.SubscriptionPlanDto.EndDate;
        subscriptionPlanEntity.UpdatedBy = request.SubscriptionPlanDto.UpdatedBy;

        await _unitOfWork.SubscriptionPlanRepository.UpdateAsync(subscriptionPlanEntity);
        await _unitOfWork.SaveChangesAsync();


        #region ActivityLog

        var currentUserId = _userContext.GetCurrentUserId();

        await _activityLogger.LogAsync(
                userId: currentUserId.Value,
                action: "Update",
                entityType: "SubscriptionPlan",
                entityId: subscriptionPlanEntity.Id,
                performedBy: currentUserId,
                description: $"User {currentUserId.Value} (ID: {currentUserId}) updated Subscription Plan: {subscriptionPlanEntity.PlanName} (ID: {subscriptionPlanEntity.Id})."
            );


        #endregion


        var responseDto = _mapper.Map<UpdateSubscriptionPlanDto>(subscriptionPlanEntity);

        return new ResponseModel<UpdateSubscriptionPlanDto>
        {
            Data = responseDto,
            IsSuccess = true
        };
    }
}