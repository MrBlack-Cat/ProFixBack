using Application.CQRS.ParentCategories.DTOs;
using Application.CQRS.ServiceTypes.DTOs;
using Application.CQRS.ServiceTypes.Queries.Requests;
using Common.GlobalResponse;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class ParentCategoryController : ControllerBase
{
    private readonly IMediator _mediator;

    public ParentCategoryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // GET: api/ParentCategory
    [HttpGet]
    public async Task<ActionResult<ResponseModel<List<ParentCategoryListDto>>>> GetAll()
    {
        var query = new GetAllParentCategoriesQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    // GET: api/ParentCategory/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<ResponseModel<ParentCategoryListDto>>> GetById(int id)
    {
        var query = new GetParentCategoryByIdQuery(id);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
