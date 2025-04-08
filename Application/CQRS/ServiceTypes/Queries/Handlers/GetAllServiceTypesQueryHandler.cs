using Application.CQRS.ServiceTypes.DTOs;
using AutoMapper;
using Common.GlobalResponse;
using MediatR;
using Repository.Common;

public class GetAllServiceTypesQueryHandler : IRequestHandler<GetAllServiceTypesQuery, ResponseModel<List<ServiceTypeListDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllServiceTypesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseModel<List<ServiceTypeListDto>>> Handle(GetAllServiceTypesQuery request, CancellationToken cancellationToken)
    {
        var list = await _unitOfWork.ServiceTypeRepository.GetAllAsync();
        var dto = _mapper.Map<List<ServiceTypeListDto>>(list);
        return new ResponseModel<List<ServiceTypeListDto>> { Data = dto, IsSuccess = true };
    }
}

public class GetServiceTypesByCategoryIdQueryHandler : IRequestHandler<GetServiceTypesByCategoryIdQuery, ResponseModel<List<ServiceTypeListDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetServiceTypesByCategoryIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseModel<List<ServiceTypeListDto>>> Handle(GetServiceTypesByCategoryIdQuery request, CancellationToken cancellationToken)
    {
        var list = await _unitOfWork.ServiceTypeRepository.GetByParentCategoryIdAsync(request.ParentCategoryId);
        var dto = _mapper.Map<List<ServiceTypeListDto>>(list);
        return new ResponseModel<List<ServiceTypeListDto>> { Data = dto, IsSuccess = true };
    }
}
