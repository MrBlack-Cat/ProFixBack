using MediatR;

namespace Application.CQRS.ServiceBookings.Commands.Complete;

public record CompleteServiceBookingCommandRequest(int Id) : IRequest;
