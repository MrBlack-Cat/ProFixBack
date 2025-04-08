using Application.CQRS.PortfolioItems.Commands.Requests;
using FluentValidation;

namespace Application.Validators.PortfolioItems;

public class DeletePortfolioItemCommandValidator : AbstractValidator<DeletePortfolioItemCommand>
{
    public DeletePortfolioItemCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Invalid portfolio item ID.");

        RuleFor(x => x.DeleteReason)
            .NotEmpty().WithMessage("Delete reason is required.")
            .MinimumLength(5).WithMessage("Delete reason must be at least 5 characters long.");
    }
}
