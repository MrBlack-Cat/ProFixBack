using Application.CQRS.ServiceProviderProfiles.Commands.Requests;
using Application.CQRS.ServiceProviderProfiles.DTOs;
using Application.CQRS.ServiceProviderProfiles.Queries.Requests;
using Common.GlobalResponse;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "ServiceProvider,Admin")]
public class ServiceProviderProfileController : ControllerBase
{
    private readonly IMediator _mediator;

    public ServiceProviderProfileController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] CreateServiceProviderProfileDto dto)
    {
        var userId = GetCurrentUserId();
        var command = new CreateServiceProviderProfileCommand(dto, userId);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateServiceProviderProfileDto dto)
    {
        var userId = GetCurrentUserId();

        if (dto.ServiceTypeIds is null || !dto.ServiceTypeIds.Any())
            return BadRequest("At least one ServiceTypeId is required.");

        var command = new UpdateServiceProviderProfileCommand(id, userId, dto);
        var result = await _mediator.Send(command);
        return Ok(result);
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, [FromQuery] string reason)
    {
        var deletedBy = GetCurrentUserId();
        var command = new DeleteServiceProviderProfileCommand(id, deletedBy, reason);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _mediator.Send(new GetServiceProviderProfileByIdQuery(id));
        return Ok(result);
    }

    [HttpGet("user")]
    public async Task<IActionResult> GetByUser()
    {
        var userId = GetCurrentUserId();
        var result = await _mediator.Send(new GetServiceProviderProfileByUserIdQuery(userId));
        return Ok(result);
    }

    [HttpGet("ListOfProfiles")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllServiceProviderProfilesQuery());
        return Ok(result);
    }

    private int GetCurrentUserId()
    {
        var claim = User.Claims.FirstOrDefault(c => c.Type.Contains("nameidentifier"));
        return claim != null ? int.Parse(claim.Value) : throw new UnauthorizedAccessException("User ID not found.");
    }

    [HttpGet("by-category/{categoryId}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetByCategory(int categoryId)
    {
        var result = await _mediator.Send(new GetServiceProvidersByCategoryIdQuery(categoryId));
        return Ok(result);
    }

}
