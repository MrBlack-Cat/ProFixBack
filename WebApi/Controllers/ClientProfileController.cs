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
using Common.GlobalResponse;
using Repository.Repositories;
using AutoMapper;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientProfileController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IUserContext _userContext;
    private readonly ICloudStorageService _cloudStorageService;
    private readonly IMapper _mapper;
    private readonly IClientProfileRepository _clientProfileRepository;

    public ClientProfileController(IClientProfileRepository clientProfileRepository,IMapper mapper, IMediator mediator, IUserContext userContext, ICloudStorageService cloudStorageService)
    {
        _clientProfileRepository = clientProfileRepository;
        _mediator = mediator;
        _userContext = userContext;
        _mapper = mapper;
        _cloudStorageService = cloudStorageService;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreateClientProfileDto dto)
    {
        var userId = _userContext.MustGetUserId();
        var command = new CreateClientProfileCommand(dto, userId);
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
        var command = new UploadClientAvatarCommand(model.File);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet("user")]
    [Authorize(Roles = "Client")]
    public async Task<IActionResult> GetByUser()
    {
        var userId = _userContext.MustGetUserId();
        var result = await _mediator.Send(new GetClientProfileByUserIdQuery(userId));
        return Ok(result);
    }




}
