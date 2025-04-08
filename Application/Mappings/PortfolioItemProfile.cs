using Application.CQRS.PortfolioItems.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

public class PortfolioItemProfile : Profile
{
    public PortfolioItemProfile()
    {
        CreateMap<PortfolioItem, GetPortfolioItemByIdDto>();
        CreateMap<PortfolioItem, PortfolioItemListDto>();
        CreateMap<CreatePortfolioItemDto, PortfolioItem>();
        CreateMap<UpdatePortfolioItemDto, PortfolioItem>();
    }
}
