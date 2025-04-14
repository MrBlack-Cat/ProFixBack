using Application.CQRS.ServiceBookings.DTOs;
using MediatR;

namespace Application.CQRS.ServiceBookings.Queries.GetById;

public record GetServiceBookingByIdQueryRequest(int Id) : IRequest<GetServiceBookingByIdDto>;
