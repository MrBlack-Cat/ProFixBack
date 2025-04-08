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
    public async Task<IActionResult> UploadClientAvatar([FromForm] UploadFileDto model)
    {
        var command = new UploadClientAvatarCommand(model.File);
        var result = await _mediator.Send(command);
        return Ok(result);
    }




    [HttpPost("upload-certificate-file")]
    [Authorize]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadCertificateFile([FromForm] UploadCertificateFileDto model)
    {
        var command = new UploadCertificateFileCommand(model.File);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("upload-guarantee-file")]
    [Authorize]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadGuaranteeFile([FromForm] UploadGuaranteeFileDto model)
    {
        var command = new UploadGuaranteeFileCommand(model.File);
        var result = await _mediator.Send(command);
        return Ok(result);
    }


}
