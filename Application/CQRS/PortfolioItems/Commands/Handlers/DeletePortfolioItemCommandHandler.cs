using Application.CQRS.PortfolioItems.Commands.Requests;
using Common.Exceptions;
using Common.GlobalResponse;
using Domain.Entities;
using MediatR;
using Repository.Common;

namespace Application.CQRS.PortfolioItems.Commands.Handlers;

public class DeletePortfolioItemCommandHandler : IRequestHandler<DeletePortfolioItemCommand, ResponseModel<string>>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeletePortfolioItemCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseModel<string>> Handle(DeletePortfolioItemCommand request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.PortfolioItemRepository.GetByIdAsync(request.Id);
        if (entity == null || entity.IsDeleted)
            throw new NotFoundException("Portfolio item not found");

        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        entity.DeletedBy = request.DeletedBy;
        entity.DeletedReason = request.DeleteReason;

        await _unitOfWork.PortfolioItemRepository.DeleteAsync(entity);

        return new ResponseModel<string>
        {
            Data = "Portfolio item deleted successfully",
            IsSuccess = true
        };
    }
}
