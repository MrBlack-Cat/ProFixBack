using Application.CQRS.Notifications.Commands.Requests;
using FluentValidation;

namespace Application.Validators.Notifications;

public class DeleteNotificationCommandValidator : AbstractValidator<DeleteNotificationCommand>
{
    public DeleteNotificationCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Notification ID is required.");

        RuleFor(x => x.Reason)
            .NotEmpty().WithMessage("Delete reason is required.")
            .MaximumLength(500).WithMessage("Delete reason must not exceed 500 characters.");
    }
}
