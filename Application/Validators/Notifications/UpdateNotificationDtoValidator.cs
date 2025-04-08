using Application.CQRS.Notifications.DTOs;
using FluentValidation;

namespace Application.Validators.Notifications;

public class UpdateNotificationDtoValidator : AbstractValidator<UpdateNotificationDto>
{
    public UpdateNotificationDtoValidator()
    {
        RuleFor(x => x.TypeId)
            .GreaterThan(0).WithMessage("Notification Type is required.");

        RuleFor(x => x.Message)
            .NotEmpty().WithMessage("Message is required.")
            .MaximumLength(1000).WithMessage("Message must be at most 1000 characters.");
    }
}
