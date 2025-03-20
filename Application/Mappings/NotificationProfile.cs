using Application.CQRS.Notifications.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

public class NotificationProfile : Profile
{
    public NotificationProfile()
    {
        CreateMap<Notification, CreateNotificationDto>().ReverseMap();
        CreateMap<Notification, UpdateNotificationDto>().ReverseMap();
        CreateMap<Notification, GetNotificationByIdDto>().ReverseMap();
        CreateMap<Notification, NotificationListDto>().ReverseMap();
    }
}
