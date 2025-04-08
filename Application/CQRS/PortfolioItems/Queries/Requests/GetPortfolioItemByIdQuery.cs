using Application.CQRS.PortfolioItems.DTOs;
using Common.GlobalResponse;
using MediatR;

namespace Application.CQRS.PortfolioItems.Queries.Requests;

public record GetPortfolioItemByIdQuery(int Id) : IRequest<ResponseModel<GetPortfolioItemByIdDto>>;
