using Application.CQRS.PortfolioItems.DTOs;
using Application.CQRS.PortfolioItems.Queries.Requests;
using AutoMapper;
using Common.Exceptions;
using Common.GlobalResponse;
using MediatR;
using Repository.Common;

namespace Application.CQRS.PortfolioItems.Queries.Handlers;

public class GetPortfolioItemByIdQueryHandler : IRequestHandler<GetPortfolioItemByIdQuery, ResponseModel<GetPortfolioItemByIdDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetPortfolioItemByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseModel<GetPortfolioItemByIdDto>> Handle(GetPortfolioItemByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.PortfolioItemRepository.GetByIdAsync(request.Id);
        if (entity == null || entity.IsDeleted)
            throw new NotFoundException("Portfolio item not found");

        var dto = _mapper.Map<GetPortfolioItemByIdDto>(entity);
        return new ResponseModel<GetPortfolioItemByIdDto> { Data = dto, IsSuccess = true };
    }
}
