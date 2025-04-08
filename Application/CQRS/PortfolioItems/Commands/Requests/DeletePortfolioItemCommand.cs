using Common.GlobalResponse;
using MediatR;

namespace Application.CQRS.PortfolioItems.Commands.Requests;

public record DeletePortfolioItemCommand(int Id, int DeletedBy, string DeleteReason) : IRequest<ResponseModel<string>>;
