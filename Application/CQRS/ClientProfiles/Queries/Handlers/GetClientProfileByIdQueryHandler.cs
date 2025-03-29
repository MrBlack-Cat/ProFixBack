using Application.CQRS.ClientProfiles.DTOs;
using Application.CQRS.ClientProfiles.Queries.Requests;
using Application.Common.Interfaces;
using AutoMapper;
using Common.Exceptions;
using Common.GlobalResponse;
using MediatR;
using Repository.Common;

namespace Application.CQRS.ClientProfiles.Queries.Handlers;

public class GetClientProfileByIdQueryHandler : IRequestHandler<GetClientProfileByIdQuery, ResponseModel<GetClientProfileByIdDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IActivityLoggerService _activityLogger;

    public GetClientProfileByIdQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IActivityLoggerService activityLogger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _activityLogger = activityLogger;
    }

    public async Task<ResponseModel<GetClientProfileByIdDto>> Handle(GetClientProfileByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.ClientProfileRepository.GetByIdAsync(request.Id);

        if (entity == null || entity.IsDeleted)
            throw new NotFoundException("Client profile not found");

        var dto = _mapper.Map<GetClientProfileByIdDto>(entity);

        await _activityLogger.LogAsync(
            userId: entity.UserId,
            action: "GetById",
            entityType: "ClientProfile",
            entityId: entity.Id
        );

        return new ResponseModel<GetClientProfileByIdDto>
        {
            Data = dto,
            IsSuccess = true
        };
    }
}
