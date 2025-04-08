using AutoMapper;
using Domain.Entities;
using Application.CQRS.Users.DTOs;
using static Application.CQRS.Users.Handlers.RegisterUserHandler;


namespace Application.Mappings;

public class UserProfile : Profile
{
    public UserProfile()
    {


        CreateMap<RegisterCommand, User>();
        CreateMap<User, RegisterUserDto>();

        CreateMap<User, UpdateUserDto>().ReverseMap();

        CreateMap<User, GetUserByIdDto>();
        CreateMap<User, GetAllUserDto>()
            .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role != null ? src.Role.Name : null));
        CreateMap<User, UserListDto>()
            .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role != null ? src.Role.Name : null));
    }
}
