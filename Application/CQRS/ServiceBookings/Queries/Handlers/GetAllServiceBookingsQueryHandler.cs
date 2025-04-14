using Application.CQRS.ServiceBookings.DTOs;
using AutoMapper;
using MediatR;
using Repository.Repositories;

namespace Application.CQRS.ServiceBookings.Queries.GetAll;

public class GetAllServiceBookingsQueryHandler : IRequestHandler<GetAllServiceBookingsQueryRequest, List<ServiceBookingListDto>>
{
    private readonly IServiceBookingRepository _serviceBookingRepository;
    private readonly IMapper _mapper;

    public GetAllServiceBookingsQueryHandler(
        IServiceBookingRepository serviceBookingRepository,
        IMapper mapper)
    {
        _serviceBookingRepository = serviceBookingRepository;
        _mapper = mapper;
    }

    public async Task<List<ServiceBookingListDto>> Handle(GetAllServiceBookingsQueryRequest request, CancellationToken cancellationToken)
    {
        var bookings = await _serviceBookingRepository.GetAllAsync();
        var activeBookings = bookings.Where(b => !b.IsDeleted).ToList();
        return _mapper.Map<List<ServiceBookingListDto>>(activeBookings);
    }
}
