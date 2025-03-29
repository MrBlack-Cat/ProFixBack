using Application.CQRS.ClientProfiles.Commands.Requests;
using Application.CQRS.ClientProfiles.DTOs;
using Application.CQRS.ClientProfiles.Queries.Requests;
using Application.Common.Interfaces;
using Common.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.CQRS.FileUploads.Commands.Requests;
using Common.Interfaces;
using Application.CQRS.FileUploads.DTOs;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientProfileController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IUserContext _userContext;
    private readonly ICloudStorageService _cloudStorageService;

    public ClientProfileController(IMediator mediator, IUserContext userContext, ICloudStorageService cloudStorageService)
    {
        _mediator = mediator;
        _userContext = userContext;
        _cloudStorageService = cloudStorageService;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreateClientProfileDto dto)
    {
        dto.UserId = _userContext.MustGetUserId();

        var command = new CreateClientProfileCommand(dto);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var query = new GetClientProfileByIdQuery(id);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateClientProfileDto dto)
    {
        int currentUserId = _userContext.MustGetUserId();

        var command = new UpdateClientProfileCommand(id, currentUserId, dto);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id, [FromQuery] string deletedReason)
    {
        var userId = _userContext.MustGetUserId();

        if (string.IsNullOrWhiteSpace(deletedReason))
            throw new BadRequestException("Delete reason is required.");

        var command = new DeleteClientProfileCommand(id, userId, deletedReason);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet("by-user/{userId}")]
    [Authorize]
    public async Task<IActionResult> GetByUserId(int userId)
    {
        var query = new GetClientProfileByUserIdQuery(userId);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("all")]
    [Authorize]
    public async Task<IActionResult> GetAll()
    {
        var query = new GetAllClientProfilesQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("upload-avatar")]
    [Authorize]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadAvatar([FromForm] UploadFileDto model)
    {
        var command = new UploadClientAvatarCommand(model.File, model.UserId);
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}
