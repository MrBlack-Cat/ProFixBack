using Application.CQRS.ActivityLogs.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

public class ActivityLogProfile : Profile
{
    public ActivityLogProfile()
    {
        CreateMap<ActivityLog, CreateActivityLogDto>().ReverseMap();
        CreateMap<ActivityLog, UpdateActivityLogDto>().ReverseMap();
        CreateMap<ActivityLog, GetActivityLogByIdDto>().ReverseMap();
        CreateMap<ActivityLog, ActivityLogListDto>().ReverseMap();
    }
}
