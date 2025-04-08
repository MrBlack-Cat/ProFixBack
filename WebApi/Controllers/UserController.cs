using Application.CQRS.Users.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Common.GlobalResponse;
using Application.CQRS.Users.Handlers;
using Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Common.Exceptions;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IUserContext _userContext;

    public UsersController(IMediator mediator, IUserContext userContext)
    {
        _mediator = mediator;
        _userContext = userContext;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
    {
        var user = _userContext.GetCurrentUser();
        if (user?.Identity is { IsAuthenticated: true })
            throw new ForbiddenException("You are already authenticated and cannot register again.");

        var command = new RegisterUserHandler.RegisterCommand(dto.UserName, dto.Email, dto.Password, dto.RoleId);
        var result = await _mediator.Send(command);
        return Ok(result);
    }




    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var command = new GetAllUserHandler.Command();
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var query = new GetUserByIdHandler.Query { Id = id };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateUserDto dto)
    {
        var currentUserId = _userContext.MustGetUserId();
        var command = new UpdateUserHandler.UpdateCommand(id, currentUserId, dto);
        var result = await _mediator.Send(command);
        return Ok(result);
    }


    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id, [FromQuery] string deletedReason)
    {
        var deletedBy = _userContext.MustGetUserId();
        var userRole = _userContext.GetUserRole();

        if (userRole != "Admin")
            throw new ForbiddenException("Only Admins can delete users.");

        if (string.IsNullOrWhiteSpace(deletedReason))
            throw new BadRequestException("Delete reason is required.");


        var command = new DeleteHandler.Command
        {
            Id = id,
            DeletedBy = deletedBy,
            DeletedReason = deletedReason
        };

        var result = await _mediator.Send(command);
        return Ok(result);
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginHandlers.LoginRequest request)
    {
        var result = await _mediator.Send(request);
        return Ok(result);
    }


}
