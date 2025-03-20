using Application.CQRS.PortfolioItems.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

public class PortfolioItemProfile : Profile
{
    public PortfolioItemProfile()
    {
        CreateMap<PortfolioItem, CreatePortfolioItemDto>().ReverseMap();
        CreateMap<PortfolioItem, UpdatePortfolioItemDto>().ReverseMap();
        CreateMap<PortfolioItem, GetPortfolioItemByIdDto>().ReverseMap();
        CreateMap<PortfolioItem, PortfolioItemListDto>().ReverseMap();
    }
}
