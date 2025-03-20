using Application.CQRS.SupportTickets.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

public class SupportTicketProfile : Profile
{
    public SupportTicketProfile()
    {
        CreateMap<SupportTicket, CreateSupportTicketDto>().ReverseMap();
        CreateMap<SupportTicket, UpdateSupportTicketDto>().ReverseMap();
        CreateMap<SupportTicket, GetSupportTicketByIdDto>().ReverseMap();
        CreateMap<SupportTicket, SupportTicketListDto>().ReverseMap();
    }
}
