using Application.CQRS.ComplaintTypes.Queries.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR; 

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComplaintTypeController : ControllerBase 
    {
        private readonly IMediator _mediator; 

        public ComplaintTypeController(IMediator mediator) 
        {
            _mediator = mediator;
        }

        [HttpGet("types")]
        [AllowAnonymous]
        public async Task<IActionResult> GetComplaintTypes()
        {
            var result = await _mediator.Send(new GetComplaintTypesQuery());
            return Ok(result);
        }
    }
}
