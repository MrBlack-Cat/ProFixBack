using Application.CQRS.Posts.DTOs;
using Application.CQRS.Posts.Handlers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PostController : ControllerBase
{
    private readonly IMediator _mediator;

    public PostController( IMediator mediator)
    {
        _mediator = mediator;   

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
    public async Task<IActionResult> UpdatePost([FromQuery] UpdatePostHandler.Command command)
    {
        var response = await _mediator.Send(command);

        if (response.IsSuccess)
        {
            return Ok(response.Data);
        }

        return BadRequest(response.Errors);
    }

//qeyd : frombody yazanda error olur sebebi ise id ni hem route dan hemde body icersinden alir buda uygun gelmir error aliriq 
//query olanda  query string de olmayan property leri null kimi qebul edir ve xeta olmur ,, eger body dede hamsi nullable olsaydi problem olmazdi 


    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> DeletePost(int id, [FromQuery] DeletePostHandler.Command command)
    {
        if (id != command.Id)
        {
            return BadRequest("ID mismatch between route parameter and request body.");
        }

        var response = await _mediator.Send(command);

        if (response.IsSuccess)
        {
            return Ok(new { message = "Post successfully deleted." });
        }

        return BadRequest(response.Errors);
    }




}
