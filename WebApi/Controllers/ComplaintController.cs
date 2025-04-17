using Application.Common.Interfaces;
using Application.CQRS.Complaints.Commands.Requests;
using Application.CQRS.Complaints.DTOs;
using Application.CQRS.Complaints.Queries.Requests;
using Common.GlobalResponse;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ComplaintController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IUserContext _userContext;

    public ComplaintController(IMediator mediator, IUserContext userContext)
    {
        _mediator = mediator;
        _userContext = userContext;
    }

    [HttpPost("create")]
    [Authorize(Roles = "Client,ServiceProvider")]
    public async Task<IActionResult> CreateComplaint([FromBody] CreateComplaintDto dto)
    {
        var fromUserId = _userContext.MustGetUserId();
        var command = new CreateComplaintCommand(fromUserId, dto);
        var result = await _mediator.Send(command);
        return Ok(result);
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _mediator.Send(new GetComplaintByIdQuery(id));
        return Ok(result);
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetList()
    {
        var result = await _mediator.Send(new GetComplaintListQuery());
        return Ok(result);
    }

    [HttpPut("mark-as-viewed/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> MarkAsViewed(int id)
    {
        var result = await _mediator.Send(new MarkComplaintAsViewedCommand(id));
        return Ok(result);
    }

    [HttpPut("mark-as-resolved/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> MarkAsResolved(int id)
    {
        var result = await _mediator.Send(new MarkComplaintAsResolvedCommand(id));
        return Ok(result);
    }


}
