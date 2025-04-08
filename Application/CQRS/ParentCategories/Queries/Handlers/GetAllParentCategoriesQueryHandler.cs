using Application.CQRS.ParentCategories.DTOs;
using AutoMapper;
using Common.GlobalResponse;
using MediatR;
using Repository.Common;

public class GetAllParentCategoriesQueryHandler : IRequestHandler<GetAllParentCategoriesQuery, ResponseModel<List<ParentCategoryListDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllParentCategoriesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseModel<List<ParentCategoryListDto>>> Handle(GetAllParentCategoriesQuery request, CancellationToken cancellationToken)
    {
        var list = await _unitOfWork.ParentCategoryRepository.GetAllAsync();
        var dto = _mapper.Map<List<ParentCategoryListDto>>(list);
        return new ResponseModel<List<ParentCategoryListDto>> { Data = dto, IsSuccess = true };
    }
}
