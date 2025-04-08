using Application.CQRS.PortfolioItems.DTOs;
using FluentValidation;

namespace Application.Validators.PortfolioItems;

public class CreatePortfolioItemDtoValidator : AbstractValidator<CreatePortfolioItemDto>
{
    public CreatePortfolioItemDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(100);

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .MaximumLength(1000);

        RuleFor(x => x.ImageUrl)
            .NotEmpty().WithMessage("Image URL is required")
            .Must(url => Uri.TryCreate(url, UriKind.Absolute, out _)).WithMessage("Image URL must be valid");
    }
}
