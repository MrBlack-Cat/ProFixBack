using Application.CQRS.SubscriptionPlans.DTOs;
using Common.GlobalResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.SubscriptionPlans.Commands.Requests;

public class CreateSubscriptionPlanRequest : IRequest<ResponseModel<CreateSubscriptionPlanDto>>
{
    public CreateSubscriptionPlanDto SubscriptionPlanDto { get; set; }

    public CreateSubscriptionPlanRequest(CreateSubscriptionPlanDto subscriptionPlanDto)
    {
        SubscriptionPlanDto = subscriptionPlanDto;
    }
}
