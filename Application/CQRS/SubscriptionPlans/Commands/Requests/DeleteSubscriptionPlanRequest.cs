using Application.CQRS.SubscriptionPlans.DTOs;
using Common.GlobalResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.SubscriptionPlans.Commands.Requests;

public class DeleteSubscriptionPlanRequest : IRequest<ResponseModel<DeleteSubscriptionPlanDto>>
{
    public DeleteSubscriptionPlanDto SubscriptionPlanDto { get; set; }

    public DeleteSubscriptionPlanRequest(DeleteSubscriptionPlanDto subscriptionPlanDto)
    {
        SubscriptionPlanDto = subscriptionPlanDto;
    }
}
