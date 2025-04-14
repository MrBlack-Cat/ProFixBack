using Application.CQRS.ServiceBookings.Queries.GetBookedSlots;
using Domain.Entities;
using MediatR;
using Repository.Repositories;

namespace Application.CQRS.ServiceBookings.Queries.GetBookedSlots;

public class GetBookedSlotsQueryHandler : IRequestHandler<GetBookedSlotsQueryRequest, List<BookedSlotDto>>
{
    private readonly IServiceBookingRepository _repository;

    public GetBookedSlotsQueryHandler(IServiceBookingRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<BookedSlotDto>> Handle(GetBookedSlotsQueryRequest request, CancellationToken cancellationToken)
    {
        var bookings = await _repository.GetByDateAsync(request.ProviderId, request.Date.Date);

        return bookings.Select(b => new BookedSlotDto
        {
            StartTime = b.StartTime.ToString(@"hh\:mm"),
            EndTime = b.EndTime.ToString(@"hh\:mm")
        }).ToList();
    }
}
