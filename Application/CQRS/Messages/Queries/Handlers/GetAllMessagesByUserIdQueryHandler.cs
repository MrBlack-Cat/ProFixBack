using Application.CQRS.Messages.DTOs;
using Application.CQRS.Messages.Queries.Requests;
using AutoMapper;
using Common.GlobalResponse;
using MediatR;
using Repository.Common;

namespace Application.CQRS.Messages.Queries.Handlers;

public class GetAllMessagesByUserIdQueryHandler : IRequestHandler<GetAllMessagesByUserIdQuery, ResponseModel<List<MessageListDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllMessagesByUserIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseModel<List<MessageListDto>>> Handle(GetAllMessagesByUserIdQuery request, CancellationToken cancellationToken)
    {
        var all = await _unitOfWork.MessageRepository.GetAllAsync();

        var messages = all
            .Where(m => (m.SenderUserId == request.UserId || m.ReceiverUserId == request.UserId) && !m.IsDeleted)
            .OrderByDescending(m => m.CreatedAt)
            .ToList();

        var result = _mapper.Map<List<MessageListDto>>(messages);

        return new ResponseModel<List<MessageListDto>> { Data = result, IsSuccess = true };
    }
}
