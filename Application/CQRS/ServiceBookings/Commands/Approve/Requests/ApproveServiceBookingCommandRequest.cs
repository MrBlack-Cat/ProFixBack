using MediatR;

namespace Application.CQRS.ServiceBookings.Commands.Approve;

public record ApproveServiceBookingCommandRequest(int Id) : IRequest;
