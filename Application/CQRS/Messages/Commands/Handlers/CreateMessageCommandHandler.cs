using Application.CQRS.Messages.Commands.Requests;
using Application.CQRS.Messages.DTOs;
using Application.Common.Interfaces;
using AutoMapper;
using Common.GlobalResponse;
using Domain.Entities;
using MediatR;
using Repository.Common;

namespace Application.CQRS.Messages.Commands.Handlers;

public class CreateMessageCommandHandler : IRequestHandler<CreateMessageCommand, ResponseModel<CreateMessageDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IActivityLoggerService _activityLogger;

    public CreateMessageCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IActivityLoggerService activityLogger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _activityLogger = activityLogger;
    }

    public async Task<ResponseModel<CreateMessageDto>> Handle(CreateMessageCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Dto;

        var entity = new Message
        {
            SenderUserId = request.SenderUserId,
            ReceiverUserId = dto.ReceiverUserId,
            Content = dto.Content,
            IsRead = false,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = request.SenderUserId
        };

        await _unitOfWork.MessageRepository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();

        await _activityLogger.LogAsync(
            userId: request.SenderUserId,
            action: "Send",
            entityType: "Message",
            entityId: entity.Id,
            performedBy: request.SenderUserId
        );

        var result = _mapper.Map<CreateMessageDto>(entity);
        return new ResponseModel<CreateMessageDto> { Data = result, IsSuccess = true };
    }
}
