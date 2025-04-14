using Application.CQRS.ServiceBookings.DTOs;
using MediatR;

namespace Application.CQRS.ServiceBookings.Queries.GetAll;

public record GetAllServiceBookingsQueryRequest() : IRequest<List<ServiceBookingListDto>>;
