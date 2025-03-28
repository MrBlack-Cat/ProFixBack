using Application.CQRS.Messages.DTOs;
using FluentValidation;

namespace Application.Validators.Messages;

public class DeleteMessageDtoValidator : AbstractValidator<DeleteMessageDto>
{
    public DeleteMessageDtoValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Message ID is required.");

        RuleFor(x => x.Reason)
            .NotEmpty().WithMessage("Delete reason is required.")
            .MaximumLength(500).WithMessage("Delete reason cannot exceed 500 characters.");
    }
}
