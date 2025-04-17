using Application.Common.Interfaces;
using Application.CQRS.Reviews.Commands.Requests;
using Application.CQRS.Reviews.DTOs;
using Application.CQRS.Reviews.Queries.Requests;
using Common.GlobalResponse;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.Repositories;
using static Application.CQRS.Reviews.Handlers.DeleteReviewHandler;
using static Application.CQRS.Reviews.Handlers.GetReviewByIdHandler;
using static Application.CQRS.Reviews.Handlers.ReviewListHandler;
using static Application.CQRS.Reviews.Handlers.UpdateReviewHandler;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IUserContext _userContext;
        private readonly IClientProfileRepository _clientProfileRepository;
        private readonly IReviewRepository _reviewRepository;

        public ReviewController(IMediator mediator, IUserContext userContext, IClientProfileRepository clientProfileRepository, IReviewRepository reviewRepository)
        {
            _mediator = mediator;
            _userContext = userContext;
            _clientProfileRepository = clientProfileRepository;
            _reviewRepository = reviewRepository;
        }



        //[HttpPost("Create")]
        //public async Task<ActionResult<ResponseModel<CreateReviewDto>>> Create([FromBody] CreateReviewDto dto)
        //{
        //    if (dto == null || dto.ClientProfileId <= 0 || dto.ServiceProviderProfileId <= 0)
        //        return BadRequest(new ResponseModel<CreateReviewDto>
        //        {
        //            IsSuccess = false,
        //            Errors = ["Invalid data."]
        //        });

        //    var userId = _userContext.MustGetUserId();
        //    var command = new CreateReviewCommand(userId, dto);
        //    var result = await _mediator.Send(command);

        //    return result.IsSuccess ? Ok(result) : BadRequest(result);
        //}

        [HttpPost("Create")]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> CreateReview([FromBody] CreateReviewCommandDto dto)
        {
            var userId = _userContext.MustGetUserId();

            var clientProfile = await _clientProfileRepository.GetByUserIdAsync(userId);
            if (clientProfile == null)
                return BadRequest("Client profile not found");

            var command = new CreateReviewCommand(
                clientProfile.Id,
                dto.ServiceProviderProfileId,
                dto.Rating,
                dto.Comment,
                $"{clientProfile.Name} {clientProfile.Surname}",
                clientProfile.AvatarUrl
            );

            var result = await _mediator.Send(command);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }





        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, [FromQuery] string reason)
        {
            if (string.IsNullOrWhiteSpace(reason))
                return BadRequest("Delete reason is required.");

            var userId = _userContext.MustGetUserId();
            var command = new DeleteReviewCommand(id, userId, reason);
            var result = await _mediator.Send(command);
            return Ok(result);
        }




        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<ResponseModel<GetReviewByIdDto>>> GetById(int id)
        {
            if (id <= 0)
                return BadRequest(new ResponseModel<GetReviewByIdDto>
                {
                    IsSuccess = false,
                    Errors = ["Invalid ID."]
                });

            var query = new GetReviewByIdQuery(id);
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("ListOfReviews")]
        public async Task<ActionResult<ResponseModel<List<ReviewListDto>>>> GetAll()
        {
            var query = new GetReviewListQuery();
            var result = await _mediator.Send(query);

            return result.IsSuccess ? Ok(result) : NotFound(result);
        }


        [HttpPut("Update/{id}")]
        public async Task<ActionResult<ResponseModel<UpdateReviewDto>>> Update(int id, [FromBody] UpdateReviewDto dto)
        {
            if (id <= 0 || dto.Rating < 1 || dto.Rating > 5)
                return BadRequest(new ResponseModel<UpdateReviewDto>
                {
                    IsSuccess = false,
                    Errors = ["Invalid data."]
                });

            var userId = _userContext.MustGetUserId();
            var command = new UpdateReviewCommand(id, userId, dto); 
            var result = await _mediator.Send(command);

            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpGet("by-provider/{providerId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByProvider(int providerId)
        {
            var query = new GetReviewsByServiceProviderIdQuery(providerId);
            var result = await _mediator.Send(query);
            return result.IsSuccess ? Ok(result) :  NotFound(result);
        }

        [HttpGet("average-rating/{providerId}")]
        public async Task<ActionResult<ResponseModel<double>>> GetAverageRating(int providerId)
        {
            var result = await _mediator.Send(new GetAverageRatingQuery(providerId));
            return Ok(result);
        }

        [HttpGet("has-reviewed")]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> HasClientReviewed([FromQuery] int providerId)
        {
            var userId = _userContext.MustGetUserId();
            var clientProfile = await _clientProfileRepository.GetByUserIdAsync(userId);
            if (clientProfile == null)
                return BadRequest("Client profile not found");

            var existingReview = await _reviewRepository.GetByClientAndProviderAsync(clientProfile.Id, providerId);
            return Ok(new
            {
                hasReviewed = existingReview != null
            });
        }


    }
}
