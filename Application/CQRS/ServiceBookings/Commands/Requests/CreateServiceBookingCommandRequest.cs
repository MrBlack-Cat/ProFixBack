using Application.CQRS.ServiceBookings.DTOs;
using Common.GlobalResponse;
using MediatR;

namespace Application.CQRS.ServiceBookings.Commands.Create;

public record CreateServiceBookingCommandRequest(CreateServiceBookingDto Dto)
    : IRequest<ResponseModel<int>>;
