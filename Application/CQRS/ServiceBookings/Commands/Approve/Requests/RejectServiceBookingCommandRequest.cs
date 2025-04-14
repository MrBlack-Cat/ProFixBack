using MediatR;

namespace Application.CQRS.ServiceBookings.Commands.Reject;

public record RejectServiceBookingCommandRequest(int Id) : IRequest;
