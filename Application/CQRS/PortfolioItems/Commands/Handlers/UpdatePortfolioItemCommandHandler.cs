using Application.CQRS.PortfolioItems.Commands.Requests;
using Common.Exceptions;
using Common.GlobalResponse;
using MediatR;
using Repository.Common;

namespace Application.CQRS.PortfolioItems.Commands.Handlers;

public class UpdatePortfolioItemCommandHandler : IRequestHandler<UpdatePortfolioItemCommand, ResponseModel<string>>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdatePortfolioItemCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseModel<string>> Handle(UpdatePortfolioItemCommand request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.PortfolioItemRepository.GetByIdAsync(request.Id);
        if (entity == null || entity.IsDeleted)
            throw new NotFoundException("Portfolio item not found");

        entity.Title = request.Dto.Title;
        entity.Description = request.Dto.Description;
        entity.ImageUrl = request.Dto.ImageUrl;
        entity.UpdatedAt = DateTime.UtcNow;
        entity.UpdatedBy = request.CurrentUserId;

        await _unitOfWork.PortfolioItemRepository.UpdateAsync(entity);

        return new ResponseModel<string>
        {
            Data = "Portfolio item updated successfully",
            IsSuccess = true
        };
    }
}
