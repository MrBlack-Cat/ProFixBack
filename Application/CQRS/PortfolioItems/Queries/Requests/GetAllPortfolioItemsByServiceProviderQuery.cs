using Application.CQRS.PortfolioItems.DTOs;
using Common.GlobalResponse;
using MediatR;

namespace Application.CQRS.PortfolioItems.Queries.Requests;

public record GetAllPortfolioItemsByServiceProviderQuery(int CurrentUserId) : IRequest<ResponseModel<List<PortfolioItemListDto>>>;
