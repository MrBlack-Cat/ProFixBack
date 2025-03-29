using Application.CQRS.SubscriptionPlans.DTOs;
using Common.GlobalResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.SubscriptionPlans.Queries.Requests;

public class GetSubscriptionPlanByIdRequest : IRequest<ResponseModel<GetSubscriptionPlanByIdDto>>
{
    public int Id { get; set; }

    public GetSubscriptionPlanByIdRequest(int id)
    {
        Id = id;
    }
}
