using Application.CQRS.SubscriptionPlans.DTOs;
using Common.GlobalResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.SubscriptionPlans.Commands.Requests;

public class UpdateSubscriptionPlanRequest : IRequest<ResponseModel<UpdateSubscriptionPlanDto>>
{
    public UpdateSubscriptionPlanDto SubscriptionPlanDto { get; set; }

    public UpdateSubscriptionPlanRequest(UpdateSubscriptionPlanDto subscriptionPlanDto)
    {
        SubscriptionPlanDto = subscriptionPlanDto;
    }
}
