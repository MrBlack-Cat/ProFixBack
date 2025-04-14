using Application.CQRS.ServiceBookings.DTOs;
using AutoMapper;
using Common.Exceptions;
using MediatR;
using Repository.Repositories;

namespace Application.CQRS.ServiceBookings.Queries.GetById;

public class GetServiceBookingByIdQueryHandler : IRequestHandler<GetServiceBookingByIdQueryRequest, GetServiceBookingByIdDto>
{
    private readonly IServiceBookingRepository _serviceBookingRepository;
    private readonly IMapper _mapper;

    public GetServiceBookingByIdQueryHandler(
        IServiceBookingRepository serviceBookingRepository,
        IMapper mapper)
    {
        _serviceBookingRepository = serviceBookingRepository;
        _mapper = mapper;
    }

    public async Task<GetServiceBookingByIdDto> Handle(GetServiceBookingByIdQueryRequest request, CancellationToken cancellationToken)
    {
        var booking = await _serviceBookingRepository.GetDetailedByIdAsync(request.Id);

        if (booking is null || booking.IsDeleted)
            throw new NotFoundException("Service booking not found");

        return _mapper.Map<GetServiceBookingByIdDto>(booking);
    }
}
