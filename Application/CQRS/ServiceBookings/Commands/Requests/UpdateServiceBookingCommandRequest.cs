using Application.CQRS.ServiceBookings.DTOs;
using MediatR;

namespace Application.CQRS.ServiceBookings.Commands.Update;

public record UpdateServiceBookingCommandRequest(int Id, UpdateServiceBookingDto Dto) : IRequest;
