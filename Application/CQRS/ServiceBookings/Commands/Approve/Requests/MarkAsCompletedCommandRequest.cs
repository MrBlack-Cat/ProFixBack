using MediatR;

namespace Application.CQRS.ServiceBookings.Commands.Complete;

public record MarkAsCompletedCommandRequest(int Id) : IRequest;
