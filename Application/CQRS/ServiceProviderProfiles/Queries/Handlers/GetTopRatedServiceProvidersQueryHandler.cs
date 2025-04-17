using Application.CQRS.ServiceProviderProfiles.DTOs;
using Application.CQRS.ServiceProviderProfiles.Queries.Requests;
using Common.GlobalResponse;
using MediatR;
using Repository.Repositories;

namespace Application.CQRS.ServiceProviderProfiles.Queries.Handlers;

public class GetTopRatedServiceProvidersQueryHandler : IRequestHandler<GetTopRatedServiceProvidersQuery, ResponseModel<List<ServiceProviderProfileTopDto>>>
{
    private readonly IServiceProviderProfileRepository _repository;

    public GetTopRatedServiceProvidersQueryHandler(IServiceProviderProfileRepository repository)
    {
        _repository = repository;
    }

    public async Task<ResponseModel<List<ServiceProviderProfileTopDto>>> Handle(GetTopRatedServiceProvidersQuery request, CancellationToken cancellationToken)
    {
        var rawProviders = await _repository.GetTopRatedServiceProvidersRawAsync();

        var providers = rawProviders.Select(p => new ServiceProviderProfileTopDto
        {
            Id = p.Id,
            Name = p.Name,
            Surname = p.Surname,
            City = p.City,
            Age = p.Age,
            ExperienceYears = p.ExperienceYears,
            AvatarUrl = p.AvatarUrl,
            AverageRating = (double)p.AverageRating,
            ParentCategoryName = p.ParentCategoryName
        }).ToList();

        return ResponseModel<List<ServiceProviderProfileTopDto>>.Success(providers);
    }
}
