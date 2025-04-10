//using Application.CQRS.Reviews.Commands.Requests;
//using Application.CQRS.Reviews.DTOs;
//using AutoMapper;
//using Common.GlobalResponse;
//using Domain.Entities;
//using MediatR;
//using Repository.Repositories;
////using static Application.CQRS.Reviews.Handlers.CreateReviewHandler;

//namespace Application.CQRS.Reviews.Commands.Handlers;

//public class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand, ResponseModel<ReviewDto>>
//{
//    private readonly IReviewRepository _repository;
//    private readonly IMapper _mapper;

//    public CreateReviewCommandHandler(IReviewRepository repository, IMapper mapper)
//    {
//        _repository = repository;
//        _mapper = mapper;
//    }

//    public async Task<ResponseModel<ReviewDto>> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
//    {
//        var entity = new Review
//        {
//            ClientProfileId = request.ClientProfileId,
//            ServiceProviderProfileId = request.ServiceProviderProfileId,
//            Rating = request.Rating,
//            Comment = request.Comment,
//            ClientName = request.ClientName,
//            ClientAvatarUrl = request.ClientAvatarUrl,
//            CreatedAt = DateTime.UtcNow,
//            CreatedBy = request.ClientProfileId
//        };

//        await _repository.AddAsync(entity);

//        var dto = _mapper.Map<ReviewDto>(entity);

//        return new ResponseModel<ReviewDto>
//        {
//            IsSuccess = true,
//            Data = dto,
//            Errors = new List<string> { "Review created successfully." }
//        };
//    }
//}


using Application.CQRS.Reviews.Commands.Requests;
using Application.CQRS.Reviews.DTOs;
using AutoMapper;
using Common.GlobalResponse;
using Domain.Entities;
using MediatR;
using Repository.Repositories;

namespace Application.CQRS.Reviews.Commands.Handlers;

public class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand, ResponseModel<ReviewDto>>
{
    private readonly IReviewRepository _repository;
    private readonly IMapper _mapper;

    public CreateReviewCommandHandler(IReviewRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ResponseModel<ReviewDto>> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
    {
        // 🔒 Проверка: уже оставлен отзыв?
        var existing = await _repository.GetByClientAndProviderAsync(
            request.ClientProfileId,
            request.ServiceProviderProfileId
        );

        if (existing != null)
        {
            return new ResponseModel<ReviewDto>
            {
                IsSuccess = false,
                Errors = new List<string> { "You have already reviewed this provider." }
            };
        }

        // ✍️ Создание нового отзыва
        var entity = new Review
        {
            ClientProfileId = request.ClientProfileId,
            ServiceProviderProfileId = request.ServiceProviderProfileId,
            Rating = request.Rating,
            Comment = request.Comment,
            ClientName = request.ClientName,
            ClientAvatarUrl = request.ClientAvatarUrl,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = request.ClientProfileId
        };

        await _repository.AddAsync(entity);

        var dto = _mapper.Map<ReviewDto>(entity);

        return new ResponseModel<ReviewDto>
        {
            IsSuccess = true,
            Data = dto,
            Errors = new List<string> { "Review created successfully." }
        };
    }
}
