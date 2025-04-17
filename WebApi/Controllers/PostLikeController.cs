using Application.CQRS.PostLikes.Commands.Requests;
using Application.CQRS.PostLikes.Queries.Requests;
using Common.GlobalResponse;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PostLikeController : ControllerBase
{
    private readonly IMediator _mediator;

    public PostLikeController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("{postId}/like")]
    public async Task<IActionResult> LikePost(int postId)
    {
        var command = new LikePostCommand(postId);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{postId}/unlike")]
    public async Task<IActionResult> UnlikePost(int postId)
    {
        var command = new UnlikePostCommand(postId);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet("{postId}/likes-count")]
    public async Task<IActionResult> GetLikesCount(int postId)
    {
        var query = new GetPostLikesCountQuery(postId);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}