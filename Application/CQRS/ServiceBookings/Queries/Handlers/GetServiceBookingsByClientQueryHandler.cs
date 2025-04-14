using Application.Common.Interfaces;
using Application.CQRS.ServiceBookings.DTOs;
using AutoMapper;
using Common.Exceptions;
using MediatR;
using Repository.Repositories;

namespace Application.CQRS.ServiceBookings.Queries.GetByClient;

public class GetServiceBookingsByClientQueryHandler : IRequestHandler<GetServiceBookingsByClientQueryRequest, List<ServiceBookingListDto>>
{
    private readonly IUserContext _userContext;
    private readonly IClientProfileRepository _clientProfileRepository;
    private readonly IServiceBookingRepository _serviceBookingRepository;
    private readonly IMapper _mapper;

    public GetServiceBookingsByClientQueryHandler(
        IUserContext userContext,
        IClientProfileRepository clientProfileRepository,
        IServiceBookingRepository serviceBookingRepository,
        IMapper mapper)
    {
        _userContext = userContext;
        _clientProfileRepository = clientProfileRepository;
        _serviceBookingRepository = serviceBookingRepository;
        _mapper = mapper;
    }

    public async Task<List<ServiceBookingListDto>> Handle(GetServiceBookingsByClientQueryRequest request, CancellationToken cancellationToken)
    {
        var userId = _userContext.MustGetUserId();
        var client = await _clientProfileRepository.GetByUserIdAsync(userId);

        if (client is null)
            throw new NotFoundException("Client profile not found");

        var bookings = await _serviceBookingRepository.GetByClientProfileIdAsync(client.Id);

        return _mapper.Map<List<ServiceBookingListDto>>(bookings);
    }
}
