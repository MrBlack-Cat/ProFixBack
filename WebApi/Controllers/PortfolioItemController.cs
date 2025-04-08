using Application.CQRS.PortfolioItems.Commands.Requests;
using Application.CQRS.PortfolioItems.DTOs;
using Application.CQRS.PortfolioItems.Queries.Requests;
using Application.Common.Interfaces;
using Common.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PortfolioItemController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IUserContext _userContext;

    public PortfolioItemController(IMediator mediator, IUserContext userContext)
    {
        _mediator = mediator;
        _userContext = userContext;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreatePortfolioItemDto dto)
    {
        var currentUserId = _userContext.MustGetUserId();
        var command = new CreatePortfolioItemCommand(dto, currentUserId);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetById(int id)
    {
        var query = new GetPortfolioItemByIdQuery(id);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("my")]
    [Authorize]
    public async Task<IActionResult> GetAllByCurrentUser()
    {
        var userId = _userContext.MustGetUserId();
        var query = new GetAllPortfolioItemsByServiceProviderQuery(userId);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> Update(int id, [FromBody] UpdatePortfolioItemDto dto)
    {
        var currentUserId = _userContext.MustGetUserId();
        var command = new UpdatePortfolioItemCommand(id, currentUserId, dto);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id, [FromQuery] string reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
            throw new BadRequestException("Delete reason is required.");

        var userId = _userContext.MustGetUserId();
        var command = new DeletePortfolioItemCommand(id, userId, reason);
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}
