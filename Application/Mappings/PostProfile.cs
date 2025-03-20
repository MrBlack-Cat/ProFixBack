using Application.CQRS.Posts.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

public class PostProfile : Profile
{
    public PostProfile()
    {
        CreateMap<Post, CreatePostDto>().ReverseMap();
        CreateMap<Post, UpdatePostDto>().ReverseMap();
        CreateMap<Post, GetPostByIdDto>().ReverseMap();
        CreateMap<Post, PostListDto>().ReverseMap();
    }
}
