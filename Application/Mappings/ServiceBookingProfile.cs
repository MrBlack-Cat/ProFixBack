using Application.CQRS.ServiceBookings.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

public class ServiceBookingProfile : Profile
{
    public ServiceBookingProfile()
    {
        CreateMap<ServiceBooking, CreateServiceBookingDto>().ReverseMap();
        CreateMap<ServiceBooking, UpdateServiceBookingDto>().ReverseMap();
        CreateMap<ServiceBooking, GetServiceBookingByIdDto>().ReverseMap();
        CreateMap<ServiceBooking, ServiceBookingListDto>().ReverseMap();
    }
}
