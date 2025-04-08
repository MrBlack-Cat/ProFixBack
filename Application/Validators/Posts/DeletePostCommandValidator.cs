using Application.CQRS.Posts.Commands.Handlers;
using FluentValidation;

namespace Application.Validators.Posts;

public class DeletePostCommandValidator : AbstractValidator<DeletePostCommandHandler.Command>
{
    public DeletePostCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Post ID is required.");

        RuleFor(x => x.DeletedBy)
            .GreaterThan(0).WithMessage("DeletedBy is required.");

        RuleFor(x => x.DeletedReason)
            .NotEmpty().WithMessage("Reason is required.")
            .MaximumLength(500).WithMessage("Reason must not exceed 500 characters.");
    }
}
