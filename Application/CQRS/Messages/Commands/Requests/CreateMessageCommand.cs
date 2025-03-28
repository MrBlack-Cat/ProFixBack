using Application.CQRS.Messages.DTOs;
using Common.GlobalResponse;
using MediatR;

public record CreateMessageCommand(CreateMessageDto Dto, int SenderUserId)
    : IRequest<ResponseModel<CreateMessageDto>>;
