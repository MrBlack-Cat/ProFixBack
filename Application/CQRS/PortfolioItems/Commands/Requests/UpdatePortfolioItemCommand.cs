using Application.CQRS.PortfolioItems.DTOs;
using Common.GlobalResponse;
using MediatR;

namespace Application.CQRS.PortfolioItems.Commands.Requests;

public record UpdatePortfolioItemCommand(int Id, int CurrentUserId, UpdatePortfolioItemDto Dto)
    : IRequest<ResponseModel<string>>;
