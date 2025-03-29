using Application.Common.Interfaces;
using Application.CQRS.ClientProfiles.DTOs;
using Application.CQRS.ClientProfiles.Queries.Requests;
using AutoMapper;
using Common.GlobalResponse;
using MediatR;
using Repository.Common;

namespace Application.CQRS.ClientProfiles.Queries.Handlers;

public class GetAllClientProfilesQueryHandler : IRequestHandler<GetAllClientProfilesQuery, ResponseModel<List<ClientProfileDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IActivityLoggerService _activityLogger;
    private readonly IUserContext _userContext;




    public GetAllClientProfilesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IActivityLoggerService activityLogger, IUserContext userContext)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _activityLogger = activityLogger;
        _userContext = userContext;


    }

    public async Task<ResponseModel<List<ClientProfileDto>>> Handle(GetAllClientProfilesQuery request, CancellationToken cancellationToken)
    {
        var profiles = await _unitOfWork.ClientProfileRepository.GetAllAsync();
        var result = _mapper.Map<List<ClientProfileDto>>(profiles);
        int userId = _userContext.MustGetUserId();

        await _activityLogger.LogAsync(
            userId: userId,
            action: "GetAll",
            entityType: "ClientProfile",
            entityId: 0,
            performedBy: userId
        );


        return new ResponseModel<List<ClientProfileDto>>
        {
            Data = result,
            IsSuccess = true
        };
    }
}
