using Application.CQRS.Messages.DTOs;
using FluentValidation;

namespace Application.Validators.Messages;

public class CreateMessageDtoValidator : AbstractValidator<CreateMessageDto>
{
    public CreateMessageDtoValidator()
    {
        RuleFor(x => x.ReceiverUserId)
            .GreaterThan(0).WithMessage("ReceiverUserId is required.");

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Message content is required.")
            .MaximumLength(1000).WithMessage("Message content cannot exceed 1000 characters.");
    }
}
