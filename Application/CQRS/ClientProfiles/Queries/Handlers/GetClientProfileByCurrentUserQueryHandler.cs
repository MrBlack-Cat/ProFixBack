using Application.CQRS.ClientProfiles.Queries.Requests;
using Common.Exceptions;
using Common.GlobalResponse;
using Domain.Entities;
using MediatR;
using Repository.Common;

public class GetClientProfileByCurrentUserQueryHandler : IRequestHandler<GetClientProfileByCurrentUserQuery, ResponseModel<ClientProfile>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetClientProfileByCurrentUserQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseModel<ClientProfile>> Handle(GetClientProfileByCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var profile = await _unitOfWork.ClientProfileRepository.GetByUserIdAsync(request.UserId);

        if (profile == null)
            throw new NotFoundException("Client profile not found");

        return new ResponseModel<ClientProfile>
        {
            Data = profile,
            IsSuccess = true
        };
    }
}
