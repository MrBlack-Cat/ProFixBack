using Application.CQRS.Posts.Commands.Handlers;
using FluentValidation;

namespace Application.Validators.Posts;

public class CreatePostCommandValidator : AbstractValidator<CreatePostHandler.Command>
{
    public CreatePostCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200);

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Content is required.");

        RuleFor(x => x.ImageUrl)
            .MaximumLength(500).WithMessage("Image URL must not exceed 500 characters.");
    }
}
