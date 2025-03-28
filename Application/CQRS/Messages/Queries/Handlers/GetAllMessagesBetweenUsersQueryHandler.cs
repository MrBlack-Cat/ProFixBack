using Application.CQRS.Messages.DTOs;
using Application.CQRS.Messages.Queries.Requests;
using AutoMapper;
using Common.GlobalResponse;
using MediatR;
using Repository.Common;

namespace Application.CQRS.Messages.Queries.Handlers;

public class GetAllMessagesBetweenUsersQueryHandler : IRequestHandler<GetAllMessagesBetweenUsersQuery, ResponseModel<List<MessageListDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllMessagesBetweenUsersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseModel<List<MessageListDto>>> Handle(GetAllMessagesBetweenUsersQuery request, CancellationToken cancellationToken)
    {
        var messages = await _unitOfWork.MessageRepository.GetAllAsync();

        var filtered = messages
            .Where(m =>
                ((m.SenderUserId == request.UserId1 && m.ReceiverUserId == request.UserId2) ||
                 (m.SenderUserId == request.UserId2 && m.ReceiverUserId == request.UserId1)) &&
                !m.IsDeleted)
            .OrderBy(m => m.CreatedAt)
            .ToList();

        var result = _mapper.Map<List<MessageListDto>>(filtered);

        return new ResponseModel<List<MessageListDto>> { Data = result, IsSuccess = true };
    }
}
