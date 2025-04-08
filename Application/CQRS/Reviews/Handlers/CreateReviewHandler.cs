using Application.Common.Interfaces;
using Application.CQRS.Reviews.DTOs;
using AutoMapper;
using Common.Exceptions;
using Common.GlobalResponse;
using Domain.Entities;
using MediatR;
using Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Reviews.Handlers;

public class CreateReviewHandler
{
    public record struct CreateReviewCommand(int CreatedByUserId, CreateReviewDto Dto)
        : IRequest<ResponseModel<CreateReviewDto>>;

    public sealed class Handler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IActivityLoggerService activityLogger)
        : IRequestHandler<CreateReviewCommand, ResponseModel<CreateReviewDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IActivityLoggerService _activityLogger = activityLogger;

        public async Task<ResponseModel<CreateReviewDto>> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
        {
            if (request.Dto.Rating < 1 || request.Dto.Rating > 5)
                throw new BadRequestException("Rating must be between 1 and 5.");

            var userId = request.CreatedByUserId;
            var dto = request.Dto;

            var newReview = _mapper.Map<Review>(dto);
            newReview.CreatedAt = DateTime.UtcNow;
            newReview.CreatedBy = userId;

            await _unitOfWork.ReviewRepository.AddAsync(newReview);
            await _unitOfWork.SaveChangesAsync();

            await _activityLogger.LogAsync(
                userId: newReview.ClientProfileId,
                action: "Create",
                entityType: "Review",
                entityId: newReview.Id,
                performedBy: userId,
                description: $"Review created for service provider {newReview.ServiceProviderProfileId} with rating {newReview.Rating}. Comment: '{newReview.Comment}'.");

            var responseDto = _mapper.Map<CreateReviewDto>(newReview);

            return new ResponseModel<CreateReviewDto>
            {
                Data = responseDto,
                IsSuccess = true,
                Errors = ["Review created successfully."]
            };
        }
    }
}
