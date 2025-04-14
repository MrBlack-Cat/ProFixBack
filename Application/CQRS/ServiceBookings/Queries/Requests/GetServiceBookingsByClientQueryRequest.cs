using Application.CQRS.ServiceBookings.DTOs;
using MediatR;

namespace Application.CQRS.ServiceBookings.Queries.GetByClient;

public record GetServiceBookingsByClientQueryRequest() : IRequest<List<ServiceBookingListDto>>;
