using Application.CQRS.ServiceTypes.DTOs;
using Application.CQRS.ServiceTypes.Queries.Requests;
using AutoMapper;
using Common.GlobalResponse;
using MediatR;
using Repository.Common;

namespace Application.CQRS.ServiceTypes.Queries.Handlers;

public class GetServiceTypesByCategoryNameQueryHandler : IRequestHandler<GetServiceTypesByCategoryNameQuery, ResponseModel<List<ServiceTypeListDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetServiceTypesByCategoryNameQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseModel<List<ServiceTypeListDto>>> Handle(GetServiceTypesByCategoryNameQuery request, CancellationToken cancellationToken)
    {
        var category = await _unitOfWork.ParentCategoryRepository.GetByNameAsync(request.CategoryName);
        if (category == null)
        {
            return new ResponseModel<List<ServiceTypeListDto>>
            {
                IsSuccess = false,
                Errors = [$"Category '{request.CategoryName}' not found."]
            };
        }

        var serviceTypes = await _unitOfWork.ServiceTypeRepository.GetByParentCategoryIdAsync(category.Id);
        var dto = _mapper.Map<List<ServiceTypeListDto>>(serviceTypes);

        return new ResponseModel<List<ServiceTypeListDto>> { Data = dto, IsSuccess = true };
    }
}
