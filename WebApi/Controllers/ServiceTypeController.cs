using Application.CQRS.ParentCategories.DTOs;
using Application.CQRS.ServiceTypes.DTOs;
using Application.CQRS.ServiceTypes.Queries.Requests;
using Common.GlobalResponse;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ServiceTypeController : ControllerBase
{
    private readonly IMediator _mediator;

    public ServiceTypeController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("parent-categories")]
    public async Task<ActionResult<ResponseModel<List<ParentCategoryListDto>>>> GetParentCategories()
    {
        var result = await _mediator.Send(new GetAllParentCategoriesQuery());
        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<ResponseModel<List<ServiceTypeListDto>>>> GetServiceTypes()
    {
        var result = await _mediator.Send(new GetAllServiceTypesQuery());
        return Ok(result);
    }

    [HttpGet("by-category/{parentCategoryId}")]
    public async Task<ActionResult<ResponseModel<List<ServiceTypeListDto>>>> GetByCategoryId(int parentCategoryId)
    {
        var result = await _mediator.Send(new GetServiceTypesByCategoryIdQuery(parentCategoryId));
        return Ok(result);
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchByName([FromQuery] string name)
    {
        var result = await _mediator.Send(new GetServiceTypesByNameQuery(name));
        return Ok(result);
    }

    [HttpGet("by-category")]
    public async Task<IActionResult> GetByCategoryName([FromQuery] string category)
    {
        var result = await _mediator.Send(new GetServiceTypesByCategoryNameQuery(category));
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetServiceTypeById(int id)
    {
        var result = await _mediator.Send(new GetServiceTypeByIdQuery(id));
        return Ok(result);
    }


}
