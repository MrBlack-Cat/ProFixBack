using Application.CQRS.Posts.Commands.Handlers;
using Application.CQRS.Posts.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

public class PostProfile : Profile
{
    public PostProfile()    
    {
        CreateMap<Post, CreatePostDto>().ReverseMap();

        CreateMap<Post, UpdatePostDto>().ReverseMap()
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

        CreateMap<Post, GetPostByIdDto>().ReverseMap();
        CreateMap<Post, PostListDto>().ReverseMap();

        CreateMap<Post, PostDto>().ReverseMap();

        CreateMap<CreatePostHandler.Command, Post>()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));

        CreateMap<DeletePostCommandHandler.Command, Post>()
            .ForMember(dest => dest.DeletedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(_ => true))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => false));

    }
}
