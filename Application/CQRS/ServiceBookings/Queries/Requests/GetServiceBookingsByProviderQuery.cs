using Application.CQRS.ServiceBookings.DTOs;
using Common.GlobalResponse;
using MediatR;

namespace Application.CQRS.ServiceBookings.Queries.GetByProvider;

public record GetServiceBookingsByProviderQueryRequest()
    : IRequest<ResponseModel<List<ServiceBookingListDto>>>;
