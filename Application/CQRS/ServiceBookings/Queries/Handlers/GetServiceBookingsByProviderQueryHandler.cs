using Application.Common.Interfaces;
using Application.CQRS.ServiceBookings.DTOs;
using AutoMapper;
using Common.Exceptions;
using Common.GlobalResponse;
using MediatR;
using Repository.Repositories;

namespace Application.CQRS.ServiceBookings.Queries.GetByProvider;

public class GetServiceBookingsByProviderQueryHandler
    : IRequestHandler<GetServiceBookingsByProviderQueryRequest, ResponseModel<List<ServiceBookingListDto>>>
{
    private readonly IUserContext _userContext;
    private readonly IServiceProviderProfileRepository _providerRepository;
    private readonly IServiceBookingRepository _serviceBookingRepository;
    private readonly IMapper _mapper;

    public GetServiceBookingsByProviderQueryHandler(
        IUserContext userContext,
        IServiceProviderProfileRepository providerRepository,
        IServiceBookingRepository serviceBookingRepository,
        IMapper mapper)
    {
        _userContext = userContext;
        _providerRepository = providerRepository;
        _serviceBookingRepository = serviceBookingRepository;
        _mapper = mapper;
    }

    public async Task<ResponseModel<List<ServiceBookingListDto>>> Handle(GetServiceBookingsByProviderQueryRequest request, CancellationToken cancellationToken)
    {
        var userId = _userContext.MustGetUserId();
        var provider = await _providerRepository.GetByUserIdAsync(userId);

        if (provider is null)
            throw new NotFoundException("Service provider profile not found");

        var bookings = await _serviceBookingRepository.GetByProviderProfileIdAsync(provider.Id);
        var mapped = _mapper.Map<List<ServiceBookingListDto>>(bookings);

        return ResponseModel<List<ServiceBookingListDto>>.Success(mapped);
    }
}
