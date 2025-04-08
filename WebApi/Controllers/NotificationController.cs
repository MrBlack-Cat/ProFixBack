using Application.CQRS.Notifications.Commands.Requests;
using Application.CQRS.Notifications.DTOs;
using Application.CQRS.Notifications.Queries.Requests;
using Application.Common.Interfaces;
using Common.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NotificationController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IUserContext _userContext;

    public NotificationController(IMediator mediator, IUserContext userContext)
    {
        _mediator = mediator;
        _userContext = userContext;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateNotificationDto dto)
    {
        var userId = _userContext.MustGetUserId();
        var command = new CreateNotificationCommand(dto, userId);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateNotificationDto dto)
    {
        var userId = _userContext.MustGetUserId();
        var command = new UpdateNotificationCommand(id, userId, dto);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, [FromQuery] string reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
            throw new BadRequestException("Delete reason is required.");

        var userId = _userContext.MustGetUserId();
        var command = new DeleteNotificationCommand(id, userId, reason);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var query = new GetNotificationByIdQuery(id);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("user")]
    public async Task<IActionResult> GetByUser()
    {
        var userId = _userContext.MustGetUserId();
        var query = new GetAllNotificationsByUserIdQuery(userId);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll()
    {
        var query = new GetAllNotificationsQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("unread")]
    public async Task<IActionResult> GetUnread()
    {
        var userId = _userContext.MustGetUserId();
        var query = new GetUnreadNotificationsQuery(userId);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPut("{id}/mark-as-read")]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        var userId = _userContext.MustGetUserId();
        var command = new MarkAsReadCommand(id, userId);
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}

