using Application.CQRS.ServiceProviderProfiles.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

public class ServiceProviderProfileProfile : Profile
{
    public ServiceProviderProfileProfile()
    {
        CreateMap<ServiceProviderProfile, CreateServiceProviderProfileDto>().ReverseMap();
        CreateMap<ServiceProviderProfile, UpdateServiceProviderProfileDto>().ReverseMap();
        CreateMap<ServiceProviderProfile, GetServiceProviderProfileByIdDto>().ReverseMap();
        CreateMap<ServiceProviderProfile, ServiceProviderProfileListDto>().ReverseMap();


        CreateMap<ServiceProviderProfile, GetServiceProviderProfileByIdDto>()
            .ForMember(dest => dest.GenderName, opt => opt.MapFrom(src => src.Gender != null ? src.Gender.Name : null));
    }
}
