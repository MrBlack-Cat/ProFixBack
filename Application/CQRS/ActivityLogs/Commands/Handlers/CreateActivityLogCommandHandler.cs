using Application.CQRS.ActivityLogs.Commands.Requests;
using Application.CQRS.ActivityLogs.DTOs;
using AutoMapper;
using Common.GlobalResponse;
using Domain.Entities;
using MediatR;
using Repository.Common;

namespace Application.CQRS.ActivityLogs.Commands.Handlers;

public class CreateActivityLogCommandHandler : IRequestHandler<CreateActivityLogCommand, ResponseModel<CreateActivityLogDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateActivityLogCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseModel<CreateActivityLogDto>> Handle(CreateActivityLogCommand request, CancellationToken cancellationToken)
    {
        var entity = new ActivityLog
        {
            UserId = request.Log.UserId,
            Action = request.Log.Action,
            EntityType = request.Log.EntityType,
            EntityId = request.Log.EntityId,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = request.Log.UserId
        };

        await _unitOfWork.ActivityLogRepository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();

        var result = _mapper.Map<CreateActivityLogDto>(entity);
        return new ResponseModel<CreateActivityLogDto> { Data = result, IsSuccess = true };
    }
}
