using Application.CQRS.ClientProfiles.DTOs;
using Application.CQRS.ClientProfiles.Queries.Requests;
using Application.Common.Interfaces;
using AutoMapper;
using Common.Exceptions;
using Common.GlobalResponse;
using MediatR;
using Repository.Common;
using Repository.Repositories;

namespace Application.CQRS.ClientProfiles.Queries.Handlers;

public class GetClientProfileByUserIdQueryHandler
    : IRequestHandler<GetClientProfileByUserIdQuery, ResponseModel<GetClientProfileDto>>
{
    private readonly IClientProfileRepository _repository;
    private readonly IMapper _mapper;

    public GetClientProfileByUserIdQueryHandler(IClientProfileRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ResponseModel<GetClientProfileDto>> Handle(GetClientProfileByUserIdQuery request, CancellationToken cancellationToken)
    {
        var profile = await _repository.GetByUserIdAsync(request.UserId);

        if (profile == null)
        {
            return new ResponseModel<GetClientProfileDto>
            {
                IsSuccess = false,
                Errors = new List<string> { "Client profile not found." }
            };
        }

        var dto = _mapper.Map<GetClientProfileDto>(profile);

        return new ResponseModel<GetClientProfileDto>
        {
            IsSuccess = true,
            Data = dto
        };
    }
}
