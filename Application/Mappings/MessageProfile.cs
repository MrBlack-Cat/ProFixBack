using Application.CQRS.Messages.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

public class MessageProfile : Profile
{
    public MessageProfile()
    {
        CreateMap<Message, CreateMessageDto>().ReverseMap();
        CreateMap<Message, UpdateMessageDto>().ReverseMap();
        CreateMap<Message, GetMessageByIdDto>().ReverseMap();
        CreateMap<Message, MessageListDto>().ReverseMap();

    }
}
