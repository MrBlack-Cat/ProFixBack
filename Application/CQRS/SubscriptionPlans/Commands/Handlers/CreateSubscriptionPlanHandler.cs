using Application.Common.Interfaces;
using Application.CQRS.SubscriptionPlans.Commands.Requests;
using Application.CQRS.SubscriptionPlans.DTOs;
using AutoMapper;
using Common.GlobalResponse;
using Domain.Entities;
using MediatR;
using Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.SubscriptionPlans.Commands.Handlers;


public class CreateSubscriptionPlanHandler(IUnitOfWork unitOfWork, IMapper mapper, IActivityLoggerService activityLogger, IUserContext userContext) : IRequestHandler<CreateSubscriptionPlanRequest, ResponseModel<CreateSubscriptionPlanDto>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly IActivityLoggerService _activityLogger = activityLogger;
    private readonly IUserContext _userContext = userContext;

    public async Task<ResponseModel<CreateSubscriptionPlanDto>> Handle(CreateSubscriptionPlanRequest request, CancellationToken cancellationToken)
    {
        // DTO-dan Entity-ə çevirmək
        var subscriptionPlanEntity = _mapper.Map<SubscriptionPlan>(request.SubscriptionPlanDto);

        // Yenisini verilənlər bazasına əlavə etmək
        await _unitOfWork.SubscriptionPlanRepository.AddAsync(subscriptionPlanEntity);
        await _unitOfWork.SaveChangesAsync();

        // Yaradılan Subscription Planı DTO-ya çevirmək
        var responseDto = _mapper.Map<CreateSubscriptionPlanDto>(subscriptionPlanEntity);

        var currentUserId = _userContext.GetCurrentUserId();

        await _activityLogger.LogAsync(
               userId: currentUserId.Value,
               action: "Create",
               entityType: "SubscriptionPlan",
               entityId: subscriptionPlanEntity.Id,
               performedBy: currentUserId,
               description: $"User {currentUserId} (ID: {currentUserId}) created a new Subscription Plan: {subscriptionPlanEntity.PlanName} (ID: {subscriptionPlanEntity.Id})."
           );




        // Response qaytarmaq
        return new ResponseModel<CreateSubscriptionPlanDto>
        {
            Data = responseDto,
            IsSuccess = true
        };
    }
}
