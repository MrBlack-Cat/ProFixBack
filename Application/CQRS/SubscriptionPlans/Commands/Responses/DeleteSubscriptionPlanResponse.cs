using Application.CQRS.SubscriptionPlans.DTOs;
using Common.GlobalResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Application.CQRS.SubscriptionPlans.Commands.Responses;

public class DeleteSubscriptionPlanResponse : ResponseModel<DeleteSubscriptionPlanDto>
{
    public DeleteSubscriptionPlanResponse(DeleteSubscriptionPlanDto data)
    {
        Data = data;
        IsSuccess = true;
    }
}
