using MediatR;

namespace Application.CQRS.ServiceBookings.Commands.Delete;

public record DeleteServiceBookingCommandRequest(int Id, string DeletedReason) : IRequest;
