using MediatR;
using Application.CQRS.Messages.DTOs;
using Application.CQRS.Messages.Queries.Requests;
using Repository.Common;
using Common.GlobalResponse;

public class GetChatSummariesQueryHandler : IRequestHandler<GetChatSummariesQuery, ResponseModel<List<ChatSummaryDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetChatSummariesQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseModel<List<ChatSummaryDto>>> Handle(GetChatSummariesQuery request, CancellationToken cancellationToken)
    {
        var rawChats = await _unitOfWork.MessageRepository.GetRawChatSummariesByUserIdAsync(request.UserId);

        var list = rawChats.Select(x => new ChatSummaryDto
        {
            OtherUserId = x.OtherUserId,
            OtherUserName = x.OtherUserName,
            LastMessageContent = x.LastMessageContent,
            LastMessageTime = x.LastMessageTime
        }).DistinctBy(x => x.OtherUserId).ToList();

        return new ResponseModel<List<ChatSummaryDto>> { Data = list, IsSuccess = true };
    }

}