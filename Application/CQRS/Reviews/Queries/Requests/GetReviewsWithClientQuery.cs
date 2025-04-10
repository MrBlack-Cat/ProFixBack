using Application.CQRS.Reviews.DTOs;
using Common.GlobalResponse;
using MediatR;

namespace Application.CQRS.Reviews.Queries.Requests;

public record struct GetReviewsWithClientQuery(int ProviderId)
    : IRequest<ResponseModel<List<ReviewListWithClientDto>>>;
