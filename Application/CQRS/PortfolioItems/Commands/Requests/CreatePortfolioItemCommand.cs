using Application.CQRS.PortfolioItems.DTOs;
using Common.GlobalResponse;
using MediatR;

namespace Application.CQRS.PortfolioItems.Commands.Requests;

public record CreatePortfolioItemCommand(CreatePortfolioItemDto Dto, int CurrentUserId)
    : IRequest<ResponseModel<string>>;
