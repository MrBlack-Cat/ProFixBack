using Application.CQRS.Reviews.DTOs;
using Common.GlobalResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Reviews.Commands.Requests
{
    public record CreateReviewCommand(
       int ClientProfileId,
       int ServiceProviderProfileId,
       int Rating,
       string Comment,
       string ClientName,
       string ClientAvatarUrl
   ) : IRequest<ResponseModel<ReviewDto>>;
}
