using Application.CQRS.Messages.Commands.Requests;
using Application.CQRS.Messages.DTOs;
using Application.CQRS.Messages.Queries.Requests;
using Application.Common.Interfaces;
using Common.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MessageController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IUserContext _userContext;

    public MessageController(IMediator mediator, IUserContext userContext)
    {
        _mediator = mediator;
        _userContext = userContext;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreateMessageDto dto)
    {
        var senderUserId = _userContext.MustGetUserId();
        var command = new CreateMessageCommand(dto, senderUserId);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetById(int id)
    {
        var query = new GetMessageByIdQuery(id);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id, [FromQuery] string reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
            throw new BadRequestException("Delete reason is required.");

        var userId = _userContext.MustGetUserId();
        var command = new DeleteMessageCommand(id, userId, reason);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet("user")]
    [Authorize]
    public async Task<IActionResult> GetAllByUser()
    {
        var userId = _userContext.MustGetUserId();
        var query = new GetAllMessagesByUserIdQuery(userId);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("between")]
    [Authorize]
    public async Task<IActionResult> GetAllBetweenUsers([FromQuery] int otherUserId)
    {
        var currentUserId = _userContext.MustGetUserId();
        var query = new GetAllMessagesBetweenUsersQuery(currentUserId, otherUserId);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("chats")]
    [Authorize]
    public async Task<IActionResult> GetChatList()
    {
        var userId = _userContext.MustGetUserId();
        var query = new GetChatSummariesQuery(userId);
        var result = await _mediator.Send(query);
        return Ok(result);
    }


}
