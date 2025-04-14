using MediatR;

namespace Application.CQRS.ServiceBookings.Commands.Cancel;
public record CancelBookingCommandRequest(int Id) : IRequest;
