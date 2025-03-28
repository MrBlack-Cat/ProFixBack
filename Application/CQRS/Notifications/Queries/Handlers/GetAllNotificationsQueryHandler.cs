using Application.CQRS.Notifications.DTOs;
using Application.CQRS.Notifications.Queries.Requests;
using AutoMapper;
using Common.GlobalResponse;
using MediatR;
using Repository.Common;

namespace Application.CQRS.Notifications.Queries.Handlers;

public class GetAllNotificationsQueryHandler : IRequestHandler<GetAllNotificationsQuery, ResponseModel<List<NotificationListDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllNotificationsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseModel<List<NotificationListDto>>> Handle(GetAllNotificationsQuery request, CancellationToken cancellationToken)
    {
        var list = await _unitOfWork.NotificationRepository.GetAllAsync();
        var filtered = list.Where(n => !n.IsDeleted).ToList();
        var result = _mapper.Map<List<NotificationListDto>>(filtered);

        return new ResponseModel<List<NotificationListDto>> { Data = result, IsSuccess = true };
    }
}
