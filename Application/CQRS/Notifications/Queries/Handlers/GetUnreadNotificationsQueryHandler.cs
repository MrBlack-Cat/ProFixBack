using Application.CQRS.Notifications.DTOs;
using Application.CQRS.Notifications.Queries.Requests;
using AutoMapper;
using Common.GlobalResponse;
using MediatR;
using Repository.Common;

namespace Application.CQRS.Notifications.Queries.Handlers;

public class GetUnreadNotificationsQueryHandler : IRequestHandler<GetUnreadNotificationsQuery, ResponseModel<List<NotificationListDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetUnreadNotificationsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseModel<List<NotificationListDto>>> Handle(GetUnreadNotificationsQuery request, CancellationToken cancellationToken)
    {
        var notifications = await _unitOfWork.NotificationRepository.GetUnreadByUserIdAsync(request.UserId);
        var dtoList = _mapper.Map<List<NotificationListDto>>(notifications);

        return new ResponseModel<List<NotificationListDto>>
        {
            Data = dtoList,
            IsSuccess = true
        };
    }
}
