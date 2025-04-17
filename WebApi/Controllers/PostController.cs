using Application.Common.Interfaces;
using Application.CQRS.Posts.Commands.Requests;
using Application.CQRS.Posts.DTOs;
using Application.CQRS.Posts.Queries.Requests;
using Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Application.CQRS.Posts.DTOs;
using Application.CQRS.Posts.Commands.Handlers;
using Application.CQRS.Posts.Queries.Handlers;
using static Application.CQRS.Posts.Commands.Handlers.UpdatePostHandler;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PostController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IUserContext _userContext;


    public PostController( IMediator mediator, IUserContext userContext)
    {
        _mediator = mediator;
        _userContext = userContext;

    }


    [HttpPost("CreatePost")]
    public async Task<IActionResult> CreatePost([FromBody] CreatePostHandler.Command command)
    {
        var response = await _mediator.Send(command);

        if (response.IsSuccess)
        {
            return CreatedAtAction(nameof(GetPostById), new { id = response.Data.Id }, response.Data);
        }


        //brat mende bele de bir problem var ki exceptionlar custom deil e mende commondan gelir yenui ki ...
        return BadRequest(response.Errors);
    }


    [HttpGet("GetPostById/{id}")]
    public async Task<IActionResult> GetPostById(int id)
    {
        var response = await _mediator.Send(new GetPostByIdHandler.Command(id));

        if (response.IsSuccess)
        {
            return Ok(response.Data);
        }

        return NotFound(response.Errors);
    }


    [HttpGet("PostList")]
    public async Task<IActionResult> GetPostsList()
    {
        var response = await _mediator.Send(new PostListHandler.Command());

        if (response.IsSuccess)
        {
            return Ok(response.Data);
        }

        return BadRequest(response.Errors);
    }


    [HttpPut("Update")]
    public async Task<IActionResult> UpdatePost([FromBody] UpdatePostCommand command)
    {
        var response = await _mediator.Send(command);
        return response.IsSuccess ? Ok(response.Data) : BadRequest(response.Errors);
    }


    //[HttpPut("Update")]
    //public async Task<IActionResult> UpdatePost([FromQuery] UpdatePostHandler.Command command)
    //{
    //    var response = await _mediator.Send(command);

    //    if (response.IsSuccess)
    //    {
    //        return Ok(response.Data);
    //    }

    //    return BadRequest(response.Errors);
    //}

    //qeyd : frombody yazanda error olur sebebi ise id ni hem route dan hemde body icersinden alir buda uygun gelmir error aliriq 
    //query olanda  query string de olmayan property leri null kimi qebul edir ve xeta olmur ,, eger body dede hamsi nullable olsaydi problem olmazdi 

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePost(int id, [FromQuery] DeletePostDto dto)
    {
        var userId = _userContext.MustGetUserId();

        if (string.IsNullOrWhiteSpace(dto.Reason))
            return BadRequest("Reason is required.");

        var command = new DeletePostCommandHandler.Command(id, userId, dto.Reason);
        var result = await _mediator.Send(command);

        return result.IsSuccess
            ? Ok(new { message = result.Data })
            : BadRequest(result.Errors);
    }



    //[HttpGet("provider")]
    //public async Task<IActionResult> GetPostsByProvider()
    //{
    //    var userId = _userContext.MustGetUserId(); // ← userId
    //    var response = await _mediator.Send(new GetPostsByProviderIdQuery(userId));


    //    return response.IsSuccess
    //        ? Ok(response.Data)
    //        : BadRequest(response.Errors);
    //}

    [HttpGet("provider")]
    public async Task<IActionResult> GetPostsByProvider()
    {
        var userId = _userContext.MustGetUserId(); 
        var response = await _mediator.Send(new GetPostsByProviderQuery(userId));
        return response.IsSuccess ? Ok(response.Data) : BadRequest(response.Errors);
    }


    [HttpGet("GetPostsByProvider/{providerId}")]
    public async Task<IActionResult> GetPostsByProvider(int providerId)
    {
        var response = await _mediator.Send(new GetPostsByProviderIdQuery(providerId));

        return response.IsSuccess ? Ok(response.Data) : BadRequest(response.Errors);
    }

    [HttpGet("liked-posts")]
    public async Task<IActionResult> GetPostsByLiked()
    {
        var response = await _mediator.Send(new GetPostsByLikedQuery());

        return response.IsSuccess
            ? Ok(response.Data)
            : BadRequest(response.Errors);
    }



}
