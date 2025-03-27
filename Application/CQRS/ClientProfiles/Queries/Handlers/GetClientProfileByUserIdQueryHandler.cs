using Application.CQRS.ClientProfiles.DTOs;
using Application.CQRS.ClientProfiles.Queries.Requests;
using Application.Common.Interfaces;
using AutoMapper;
using Common.Exceptions;
using Common.GlobalResponse;
using MediatR;
using Repository.Common;

namespace Application.CQRS.ClientProfiles.Queries.Handlers;

public class GetClientProfileByUserIdQueryHandler : IRequestHandler<GetClientProfileByUserIdQuery, ResponseModel<GetClientProfileByIdDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IActivityLoggerService _activityLogger;

    public GetClientProfileByUserIdQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IActivityLoggerService activityLogger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _activityLogger = activityLogger;
    }

    public async Task<ResponseModel<GetClientProfileByIdDto>> Handle(GetClientProfileByUserIdQuery request, CancellationToken cancellationToken)
    {
        var profile = await _unitOfWork.ClientProfileRepository.GetByUserIdAsync(request.UserId);
        if (profile is null)
            throw new NotFoundException("Client profile not found by user ID");

        var dto = _mapper.Map<GetClientProfileByIdDto>(profile);

        await _activityLogger.LogAsync(
            userId: request.UserId,
            action: "GetByUserId",
            entityType: "ClientProfile",
            entityId: profile.Id
        );

        return new ResponseModel<GetClientProfileByIdDto>
        {
            Data = dto,
            IsSuccess = true
        };
    }
}
