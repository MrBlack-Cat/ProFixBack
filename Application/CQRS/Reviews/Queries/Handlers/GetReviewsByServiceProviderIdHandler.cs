using Application.Common.Interfaces;
using Application.CQRS.Reviews.DTOs;
using Application.CQRS.Reviews.Queries.Requests;
using AutoMapper;
using Common.GlobalResponse;
using MediatR;
using Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Reviews.Queries.Handlers
{
    public sealed class GetReviewsByServiceProviderIdHandler(
     IUnitOfWork unitOfWork,
     IMapper mapper,
     IActivityLoggerService logger)
     : IRequestHandler<GetReviewsByServiceProviderIdQuery, ResponseModel<List<ReviewListDto>>>
    {
        public async Task<ResponseModel<List<ReviewListDto>>> Handle(GetReviewsByServiceProviderIdQuery request, CancellationToken cancellationToken)
        {
            var reviews = await unitOfWork.ReviewRepository.GetByServiceProviderIdAsync(request.ProviderId);
            var dto = mapper.Map<List<ReviewListDto>>(reviews);

            await logger.LogAsync(
                userId: request.ProviderId,
                action: "GetByProvider",
                entityType: "Review",
                entityId: 0,
                performedBy: request.ProviderId,
                description: $"Fetched {dto.Count} reviews for provider {request.ProviderId}");

            return ResponseModel<List<ReviewListDto>>.Success(dto);
        }
    }

}
