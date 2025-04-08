using Application.CQRS.ServiceTypes.DTOs;
using Application.CQRS.ServiceTypes.Queries.Requests;
using AutoMapper;
using Common.Exceptions;
using Common.GlobalResponse;
using MediatR;
using Repository.Common;

namespace Application.CQRS.ServiceTypes.Queries.Handlers;

public class GetServiceTypeByIdQueryHandler : IRequestHandler<GetServiceTypeByIdQuery, ResponseModel<ServiceTypeListDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetServiceTypeByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseModel<ServiceTypeListDto>> Handle(GetServiceTypeByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.ServiceTypeRepository.GetByIdAsync(request.Id);
        if (entity is null)
            throw new NotFoundException("Service type not found");

        var dto = _mapper.Map<ServiceTypeListDto>(entity);
        return new ResponseModel<ServiceTypeListDto> { Data = dto, IsSuccess = true };
    }
}
