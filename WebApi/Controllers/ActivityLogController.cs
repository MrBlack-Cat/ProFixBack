using Application.CQRS.ActivityLogs.Commands.Requests;
using Application.CQRS.ActivityLogs.DTOs;
using Application.CQRS.ActivityLogs.Queries.Requests;
using Common.GlobalResponse;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")] 
public class ActivityLogController : ControllerBase
{
    private readonly IMediator _mediator;

    public ActivityLogController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateActivityLogDto dto)
    {
        var command = new CreateActivityLogCommand(dto);
        var result = await _mediator.Send(command);
        return Ok(result);
    }


    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var query = new GetAllActivityLogsQuery();
        ResponseModel<List<ActivityLogListDto>> result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("by-user/{userId}")]
    public async Task<IActionResult> GetByUserId(int userId)
    {
        var query = new GetActivityLogsByUserIdQuery(userId);
        ResponseModel<List<ActivityLogListDto>> result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var query = new GetActivityLogByIdQuery(id);
        ResponseModel<GetActivityLogByIdDto> result = await _mediator.Send(query);
        return Ok(result);
    }

}
