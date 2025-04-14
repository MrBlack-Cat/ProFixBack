using MediatR;

namespace Application.CQRS.ServiceBookings.Commands.Start;
public record StartBookingCommandRequest(int Id) : IRequest;
