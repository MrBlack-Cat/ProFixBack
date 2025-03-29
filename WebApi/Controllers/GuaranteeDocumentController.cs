using Application.CQRS.GuaranteeDocuments.Commands.Requests;
using Application.CQRS.GuaranteeDocuments.DTOs;
using Application.CQRS.GuaranteeDocuments.Queries.Requests;
using Application.Common.Interfaces;
using Common.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GuaranteeDocumentController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IUserContext _userContext;

    public GuaranteeDocumentController(IMediator mediator, IUserContext userContext)
    {
        _mediator = mediator;
        _userContext = userContext;
    }


    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreateGuaranteeDocumentDto dto)
    {
        int currentUserId = _userContext.MustGetUserId();
        var command = new CreateGuaranteeDocumentCommand(dto, currentUserId);
        var result = await _mediator.Send(command);
        return Ok(result);
    }


    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateGuaranteeDocumentDto dto)
    {
        int userId = _userContext.MustGetUserId();
        var command = new UpdateGuaranteeDocumentCommand(id, userId, dto);
        var result = await _mediator.Send(command);
        return Ok(result);
    }


    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id, [FromQuery] string reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
            throw new BadRequestException("Delete reason is required.");

        int userId = _userContext.MustGetUserId();
        var command = new DeleteGuaranteeDocumentCommand(id, userId, reason);
        var result = await _mediator.Send(command);
        return Ok(result);
    }


    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetById(int id)
    {
        var query = new GetGuaranteeDocumentByIdQuery(id);
        var result = await _mediator.Send(query);
        return Ok(result);
    }


    [HttpGet("by-client/{clientProfileId}")]
    [Authorize]
    public async Task<IActionResult> GetByClient(int clientProfileId)
    {
        var query = new GetAllGuaranteesByClientIdQuery(clientProfileId);
        var result = await _mediator.Send(query);
        return Ok(result);
    }


    [HttpGet("by-provider/{serviceProviderProfileId}")]
    [Authorize]
    public async Task<IActionResult> GetByProvider(int serviceProviderProfileId)
    {
        var query = new GetAllGuaranteesByProviderIdQuery(serviceProviderProfileId);
        var result = await _mediator.Send(query);
        return Ok(result);
    }


    [HttpGet("all")]
    [Authorize]
    public async Task<IActionResult> GetAll()
    {
        var query = new GetAllGuaranteeDocumentsQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
