using Application.CQRS.Notifications.DTOs;
using Application.CQRS.Notifications.Queries.Requests;
using AutoMapper;
using Common.Exceptions;
using Common.GlobalResponse;
using MediatR;
using Repository.Common;

namespace Application.CQRS.Notifications.Queries.Handlers;

public class GetNotificationByIdQueryHandler : IRequestHandler<GetNotificationByIdQuery, ResponseModel<GetNotificationByIdDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetNotificationByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseModel<GetNotificationByIdDto>> Handle(GetNotificationByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.NotificationRepository.GetByIdAsync(request.Id);
        if (entity is null || entity.IsDeleted)
            throw new NotFoundException("Notification not found.");

        var dto = _mapper.Map<GetNotificationByIdDto>(entity);
        return new ResponseModel<GetNotificationByIdDto> { Data = dto, IsSuccess = true };
    }
}
