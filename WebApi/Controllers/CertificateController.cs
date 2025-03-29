using Application.CQRS.Certificates.Commands.Requests;
using Application.CQRS.Certificates.DTOs;
using Application.CQRS.Certificates.Queries.Requests;
using Application.Common.Interfaces;
using Common.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CertificateController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IUserContext _userContext;

    public CertificateController(IMediator mediator, IUserContext userContext)
    {
        _mediator = mediator;
        _userContext = userContext;
    }


    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreateCertificateDto dto)
    {
        var userId = _userContext.MustGetUserId();
        var command = new CreateCertificateCommand(dto, userId);
        var result = await _mediator.Send(command);
        return Ok(result);
    }


    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCertificateDto dto)
    {
        var userId = _userContext.MustGetUserId();
        var command = new UpdateCertificateCommand(id, userId, dto);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id, [FromQuery] string reason)
    {
        var userId = _userContext.MustGetUserId();

        if (string.IsNullOrWhiteSpace(reason))
            throw new BadRequestException("Delete reason is required.");

        var command = new DeleteCertificateCommand(id, userId, reason);
        var result = await _mediator.Send(command);
        return Ok(result);
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var query = new GetCertificateByIdQuery(id);
        var result = await _mediator.Send(query);
        return Ok(result);
    }


    [HttpGet("by-provider/{serviceProviderProfileId}")]
    public async Task<IActionResult> GetByServiceProvider(int serviceProviderProfileId)
    {
        var query = new GetAllCertificatesByServiceProviderQuery(serviceProviderProfileId);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        var query = new GetAllCertificatesQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }


}
