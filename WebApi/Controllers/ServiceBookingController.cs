using Application.CQRS.ServiceBookings.Commands.Create;
using Application.CQRS.ServiceBookings.Commands.Update;
using Application.CQRS.ServiceBookings.Commands.Delete;
using Application.CQRS.ServiceBookings.Queries.GetById;
using Application.CQRS.ServiceBookings.Queries.GetByClient;
using Application.CQRS.ServiceBookings.Queries.GetByProvider;
using Application.CQRS.ServiceBookings.Queries.GetAll;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.CQRS.ServiceBookings.DTOs;
using MediatR;
using Application.CQRS.ServiceBookings.Commands.Approve;
using Application.CQRS.ServiceBookings.Commands.Complete;
using Application.CQRS.ServiceBookings.Commands.Reject;
using Application.CQRS.ServiceBookings.Commands.Cancel;
using Application.CQRS.ServiceBookings.Commands.Start;
using Application.CQRS.ServiceBookings.Queries.GetBookedSlots;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ServiceBookingController : ControllerBase
{
    private readonly IMediator _mediator;

    public ServiceBookingController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("create")]
    [Authorize(Roles = "Admin,Client")]
    public async Task<IActionResult> Create([FromBody] CreateServiceBookingCommandRequest command)
    {
        var response = await _mediator.Send(command);
        if (!response.IsSuccess)
            return BadRequest(response);

        return Ok(response);
    }



    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateServiceBookingDto dto)
    {
        await _mediator.Send(new UpdateServiceBookingCommandRequest(id, dto));
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, [FromQuery] string reason)
    {
        await _mediator.Send(new DeleteServiceBookingCommandRequest(id, reason));
        return NoContent();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _mediator.Send(new GetServiceBookingByIdQueryRequest(id));
        return Ok(result);
    }

    [HttpGet("client")]
    public async Task<IActionResult> GetByClient()
    {
        var result = await _mediator.Send(new GetServiceBookingsByClientQueryRequest());
        return Ok(result);
    }

    [HttpGet("provider")]
    public async Task<IActionResult> GetByProvider()
    {
        var result = await _mediator.Send(new GetServiceBookingsByProviderQueryRequest());
        return Ok(result);
    }

    [HttpGet("all")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllServiceBookingsQueryRequest());
        return Ok(result);
    }

    [HttpPost("{id}/approve")]
    [Authorize(Roles = "ServiceProvider")]
    public async Task<IActionResult> ApproveBooking(int id)
    {
        await _mediator.Send(new ApproveServiceBookingCommandRequest(id));
        return NoContent();
    }

    [HttpPost("{id}/reject")]
    [Authorize(Roles = "ServiceProvider")]
    public async Task<IActionResult> RejectBooking(int id)
    {
        await _mediator.Send(new RejectServiceBookingCommandRequest(id));
        return NoContent();
    }

    [HttpPost("{id}/complete")]
    [Authorize(Roles = "ServiceProvider")]
    public async Task<IActionResult> CompleteBooking(int id)
    {
        await _mediator.Send(new MarkAsCompletedCommandRequest(id));
        return NoContent();
    }

    [HttpPost("{id}/cancel")]
    [Authorize]
    public async Task<IActionResult> CancelBooking(int id)
    {
        await _mediator.Send(new CancelBookingCommandRequest(id));
        return NoContent();
    }

    [HttpPost("{id}/start")]
    [Authorize(Roles = "ServiceProvider")]
    public async Task<IActionResult> StartBooking(int id)
    {
        await _mediator.Send(new StartBookingCommandRequest(id));
        return NoContent();
    }

    [HttpGet("booked-times")]
    public async Task<IActionResult> GetBookedTimes([FromQuery] int providerId, [FromQuery] DateTime date)
    {
        var result = await _mediator.Send(new GetBookedSlotsQueryRequest(providerId, date));
        return Ok(result);
    }



}
