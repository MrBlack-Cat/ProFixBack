using Application.CQRS.FileUploads.Commands.Requests;
using Application.CQRS.FileUploads.DTOs;
using Common.GlobalResponse;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FileUploadController : ControllerBase
{
    private readonly IMediator _mediator;

    public FileUploadController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("upload-client-avatar")]
    [Authorize]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadClientAvatar([FromForm] UploadClientAvatarCommand command)
    {
        ResponseModel<UploadFileResultDto> result = await _mediator.Send(command);
        return Ok(result);
    }
}
