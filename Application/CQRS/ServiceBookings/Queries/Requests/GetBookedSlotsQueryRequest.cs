using MediatR;

namespace Application.CQRS.ServiceBookings.Queries.GetBookedSlots;

public record GetBookedSlotsQueryRequest(int ProviderId, DateTime Date)
    : IRequest<List<BookedSlotDto>>;
