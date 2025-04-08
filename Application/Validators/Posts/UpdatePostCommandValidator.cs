using Application.CQRS.Posts.Commands.Handlers;
using Application.CQRS.Posts.DTOs;
using FluentValidation;

namespace Application.Validators.Posts;

public class UpdatePostCommandValidator : AbstractValidator<UpdatePostHandler.UpdatePostCommand>
{
    public UpdatePostCommandValidator()
    {
        RuleFor(x => x.PostDto.Id)
            .GreaterThan(0)
            .WithMessage("Post ID is required.");

        RuleFor(x => x.PostDto.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200);

        RuleFor(x => x.PostDto.Content)
            .NotEmpty().WithMessage("Content is required.");

        RuleFor(x => x.PostDto.ImageUrl)
            .MaximumLength(500).WithMessage("Image URL must not exceed 500 characters.");
    }
}
