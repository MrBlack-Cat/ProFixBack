using Application.CQRS.ServiceBookings.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

public class ServiceBookingProfile : Profile
{
    public ServiceBookingProfile()
    {
        // Entity → GetByIdDto
        CreateMap<ServiceBooking, GetServiceBookingByIdDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status ?? ""))
            .ForMember(dest => dest.ClientProfileId, opt => opt.MapFrom(src => src.ClientProfileId))
            .ForMember(dest => dest.ServiceProviderProfileId, opt => opt.MapFrom(src => src.ServiceProviderProfileId));

        CreateMap<ServiceBooking, ServiceBookingListDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status ?? ""))
            .ForMember(dest => dest.ClientName, opt => opt.MapFrom(src => src.ClientName))
            .ForMember(dest => dest.ClientAvatarUrl, opt => opt.MapFrom(src => src.ClientAvatarUrl))
            .ForMember(dest => dest.ClientProfileId, opt => opt.MapFrom(src => src.ClientProfileId))
            .ForMember(dest => dest.ServiceProviderName, opt => opt.MapFrom(src => src.ServiceProviderName))
            .ForMember(dest => dest.ServiceProviderAvatarUrl, opt => opt.MapFrom(src => src.ServiceProviderAvatarUrl))
            .ForMember(dest => dest.ServiceProviderProfileId, opt => opt.MapFrom(src => src.ServiceProviderProfileId));

        CreateMap<CreateServiceBookingDto, ServiceBooking>();
        CreateMap<UpdateServiceBookingDto, ServiceBooking>();
    }
}
