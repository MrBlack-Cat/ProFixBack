using Application.CQRS.ParentCategories.DTOs;
using Application.CQRS.ServiceTypes.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

public class ServiceTypeProfile : Profile
{
    public ServiceTypeProfile()
    {
        CreateMap<ParentCategory, ParentCategoryListDto>().ReverseMap();
        CreateMap<ServiceType, ServiceTypeListDto>().ReverseMap();
    }
}
