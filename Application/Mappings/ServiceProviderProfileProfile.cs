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
        CreateMap<ClientProfile, GetClientProfileDto>().ReverseMap(); 


        CreateMap<ServiceProviderProfile, GetServiceProviderProfileByIdDto>()
            .ForMember(dest => dest.GenderName, opt => opt.MapFrom(src => src.Gender != null ? src.Gender.Name : null))
            .ForMember(dest => dest.ParentCategoryName, opt => opt.MapFrom(src => src.ParentCategory != null ? src.ParentCategory.Name : null))
            .ForMember(dest => dest.ServiceTypes, opt => opt.MapFrom(src => src.ServiceTypes));


        CreateMap<ServiceProviderProfile, GetServiceProviderProfileByIdDto>()
            .ForMember(dest => dest.GenderName, opt => opt.MapFrom(src => src.GenderName))
            .ForMember(dest => dest.ParentCategoryName, opt => opt.MapFrom(src => src.ParentCategoryName))
            .ForMember(dest => dest.ServiceTypes, opt => opt.MapFrom(src => src.ServiceTypes));

        CreateMap<CreateServiceProviderProfileDto, ServiceProviderProfile>()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));

        CreateMap<ServiceProviderProfile, GetServiceProviderProfileByUserIdDto>()
            .ForMember(dest => dest.ServiceTypeIds, opt => opt.MapFrom(src => src.ServiceTypeIds)) 
            .ForMember(dest => dest.ServiceTypes, opt => opt.MapFrom(src => src.ServiceTypes))
            .ForMember(dest => dest.ParentCategoryName, opt => opt.MapFrom(src => src.ParentCategoryName))
            .ForMember(dest => dest.GenderName, opt => opt.MapFrom(src => src.GenderName))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));

        CreateMap<UpdateServiceProviderProfileDto, ServiceProviderProfile>()
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.ParentCategoryId, opt => opt.MapFrom(src => src.ParentCategoryId))
            .ForMember(dest => dest.ServiceTypeIds, opt => opt.MapFrom(src => src.ServiceTypeIds))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));





    }
}
