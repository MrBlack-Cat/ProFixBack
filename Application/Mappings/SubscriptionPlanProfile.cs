using Application.CQRS.SubscriptionPlans.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

public class SubscriptionPlanProfile : Profile
{
    public SubscriptionPlanProfile()
    {
        CreateMap<SubscriptionPlan, CreateSubscriptionPlanDto>().ReverseMap();
        CreateMap<SubscriptionPlan, UpdateSubscriptionPlanDto>().ReverseMap();
        CreateMap<SubscriptionPlan, GetSubscriptionPlanByIdDto>().ReverseMap();
        CreateMap<SubscriptionPlan, SubscriptionPlanListDto>().ReverseMap();
    }
}
