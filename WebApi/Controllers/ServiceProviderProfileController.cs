using Application.CQRS.ServiceProviderProfiles.DTOs;
using Application.CQRS.ServiceProviderProfiles.Handler;
using Common.GlobalResponse;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Application.CQRS.ServiceProviderProfiles.Handler.DeleteServiceProviderHandler;
using static Application.CQRS.ServiceProviderProfiles.Handler.GetServiceProviderProfileByIdHandler;
using static Application.CQRS.ServiceProviderProfiles.Handler.ServiceProviderProfileListHandler;
using static Application.CQRS.ServiceProviderProfiles.Handler.UpdateServiceProviderHandlerProfile;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceProviderProfileController : ControllerBase
    {
        private readonly IMediator _mediator;

        [HttpPost("Create")]
        public async Task<ActionResult<ResponseModel<CreateServiceProviderProfileDto>>> Create([FromBody] CreateServiceProviderProfileDto dto)
        {
            if (dto == null)
            {
                return BadRequest(new ResponseModel<CreateServiceProviderProfileDto>
                {
                    IsSuccess = false,
                    Errors = ["Invalid request data."]
                });
            }

            var command = new CreateServiceProviderProfileHandler.Command(dto);
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }



        [HttpDelete("Delete")]
        public async Task<ActionResult<ResponseModel<DeleteServiceProviderProfileDto>>> Delete([FromBody] DeleteServiceProviderProfileDto dto)
        {
            if (dto == null || dto.Id <= 0)
                return BadRequest(new ResponseModel<DeleteServiceProviderProfileDto>
                {
                    IsSuccess = false,
                    Errors = ["Invalid request data."]
                });

            var command = new DeleteServiceProviderProfileCommand(dto);
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }



        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<ResponseModel<GetServiceProviderProfileByIdDto>>> GetById(int id)
        {
            if (id <= 0)
                return BadRequest(new ResponseModel<GetServiceProviderProfileByIdDto>
                {
                    IsSuccess = false,
                    Errors =[ "Invalid ID."]
                });

            var query = new GetServiceProviderProfileByIdQuery(id);
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(result);
        }


        [HttpGet("ListOfProfiles")]
        public async Task<ActionResult<ResponseModel<List<ServiceProviderProfileListDto>>>> GetAll()
        {
            var query = new GetServiceProviderProfileListQuery();
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(result);
        }


        [HttpPut]
        public async Task<ActionResult<ResponseModel<UpdateServiceProviderProfileDto>>> Update([FromBody] UpdateServiceProviderProfileDto dto)
        {
            if (dto == null || dto.Id <= 0)
                return BadRequest(new ResponseModel<UpdateServiceProviderProfileDto>
                {
                    IsSuccess = false,
                    Errors =[ "Invalid data."]
                });

            var command = new UpdateServiceProviderProfileCommand(dto);
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(result);
        }
    }
}
