using Application.CQRS.ParentCategories.DTOs;
using Application.CQRS.ServiceTypes.DTOs;
using Application.CQRS.ServiceTypes.Queries.Requests;
using AutoMapper;
using Common.Exceptions;
using Common.GlobalResponse;
using MediatR;
using Repository.Common;

namespace Application.CQRS.ServiceTypes.Queries.Handlers;

public class GetParentCategoryByIdQueryHandler : IRequestHandler<GetParentCategoryByIdQuery, ResponseModel<ParentCategoryListDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetParentCategoryByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseModel<ParentCategoryListDto>> Handle(GetParentCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await _unitOfWork.ParentCategoryRepository.GetByIdAsync(request.Id);
        if (category is null)
            throw new NotFoundException("Parent category not found");

        var dto = _mapper.Map<ParentCategoryListDto>(category);
        return new ResponseModel<ParentCategoryListDto> { Data = dto, IsSuccess = true };
    }
}
