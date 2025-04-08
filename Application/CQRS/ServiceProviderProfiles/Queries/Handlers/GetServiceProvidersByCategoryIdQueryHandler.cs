using Application.CQRS.ServiceProviderProfiles.DTOs;
using AutoMapper;
using Common.GlobalResponse;
using MediatR;
using Repository.Repositories;

public class GetServiceProvidersByCategoryIdQueryHandler
    : IRequestHandler<GetServiceProvidersByCategoryIdQuery, ResponseModel<List<ServiceProviderProfileListDto>>>
{
    private readonly IServiceProviderProfileRepository _repository;
    private readonly IMapper _mapper;

    public GetServiceProvidersByCategoryIdQueryHandler(IServiceProviderProfileRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ResponseModel<List<ServiceProviderProfileListDto>>> Handle(GetServiceProvidersByCategoryIdQuery request, CancellationToken cancellationToken)
    {
        var providers = await _repository.GetByParentCategoryIdAsync(request.CategoryId);
        var dto = _mapper.Map<List<ServiceProviderProfileListDto>>(providers);

        return ResponseModel<List<ServiceProviderProfileListDto>>.Success(dto);
    }
}
