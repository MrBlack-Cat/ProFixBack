using Application.CQRS.ClientProfiles.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

public class ClientProfileProfile : Profile
{
    public ClientProfileProfile()
    {
        CreateMap<ClientProfile, CreateClientProfileDto>().ReverseMap();
        CreateMap<ClientProfile, UpdateClientProfileDto>().ReverseMap();
        CreateMap<ClientProfile, GetClientProfileByIdDto>().ReverseMap();
        CreateMap<ClientProfile, ClientProfileListDto>().ReverseMap();
        CreateMap<ClientProfile, ClientProfileDto>().ReverseMap();
        CreateMap<ClientProfile, GetClientProfileDto>()
            .ForMember(dest => dest.ClientProfileId, opt => opt.MapFrom(src => src.Id));


    }
}
