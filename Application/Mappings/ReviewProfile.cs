using Application.CQRS.Reviews.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

public class ReviewProfile : Profile
{
    public ReviewProfile()
    {
        CreateMap<Review, CreateReviewDto>().ReverseMap();
        CreateMap<Review, UpdateReviewDto>().ReverseMap();
        CreateMap<Review, GetReviewByIdDto>().ReverseMap();
        CreateMap<Review, ReviewListDto>().ReverseMap();
        CreateMap<Review, ReviewDto>().ReverseMap();

    }
}
