using Application.CQRS.ActivityLogs.DTOs;
using Application.CQRS.ActivityLogs.Queries.Requests;
using AutoMapper;
using Common.Exceptions;
using Common.GlobalResponse;
using MediatR;
using Repository.Common;

namespace Application.CQRS.ActivityLogs.Queries.Handlers;

public class GetActivityLogByIdQueryHandler : IRequestHandler<GetActivityLogByIdQuery, ResponseModel<GetActivityLogByIdDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetActivityLogByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseModel<GetActivityLogByIdDto>> Handle(GetActivityLogByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.ActivityLogRepository.GetByIdAsync(request.Id);
        if (entity is null || entity.IsDeleted)
            throw new NotFoundException("Activity log not found.");

        var dto = _mapper.Map<GetActivityLogByIdDto>(entity);

        return new ResponseModel<GetActivityLogByIdDto>
        {
            Data = dto,
            IsSuccess = true
        };
    }
}
