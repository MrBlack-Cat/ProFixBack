using Application.CQRS.PortfolioItems.DTOs;
using Application.CQRS.PortfolioItems.Queries.Requests;
using AutoMapper;
using Common.Exceptions;
using Common.GlobalResponse;
using MediatR;
using Repository.Common;

namespace Application.CQRS.PortfolioItems.Queries.Handlers;

public class GetAllPortfolioItemsByServiceProviderQueryHandler : IRequestHandler<GetAllPortfolioItemsByServiceProviderQuery, ResponseModel<List<PortfolioItemListDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllPortfolioItemsByServiceProviderQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseModel<List<PortfolioItemListDto>>> Handle(GetAllPortfolioItemsByServiceProviderQuery request, CancellationToken cancellationToken)
    {
        var profile = await _unitOfWork.ServiceProviderProfileRepository.GetByUserIdAsync(request.CurrentUserId);
        if (profile == null)
            throw new NotFoundException("Service provider profile not found");

        var items = await _unitOfWork.PortfolioItemRepository.GetByServiceProviderIdAsync(profile.Id);
        var dtoList = _mapper.Map<List<PortfolioItemListDto>>(items);

        return new ResponseModel<List<PortfolioItemListDto>> { Data = dtoList, IsSuccess = true };
    }
}
