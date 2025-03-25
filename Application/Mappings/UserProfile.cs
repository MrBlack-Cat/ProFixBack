using AutoMapper;
using Domain.Entities;
using Application.CQRS.Users.DTOs;
using static Application.CQRS.Users.Handlers.RegisterUserHandler;


namespace Application.Mappings;

public class UserProfile : Profile
{
    public UserProfile()
    {
        //CreateMap<User, RegisterUserDto>().ReverseMap();
        //CreateMap<User, UpdateUserDto>().ReverseMap();
        //CreateMap<User, GetUserByIdDto>().ReverseMap();
        //CreateMap<User, UserListDto>().ReverseMap();
        //CreateMap<User, GetAllUserDto>().ReverseMap();


        CreateMap<RegisterCommand, User>();
        CreateMap<User, RegisterUserDto>();

        CreateMap<User, UpdateUserDto>().ReverseMap();

        CreateMap<User, GetUserByIdDto>();
        CreateMap<User, GetAllUserDto>();
        CreateMap<User, UserListDto>();
    }
}
