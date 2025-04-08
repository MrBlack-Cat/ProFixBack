using Application.CQRS.Notifications.Commands.Requests;
using FluentValidation;

namespace Application.Validators.Notifications;

public class MarkAsReadCommandValidator : AbstractValidator<MarkAsReadCommand>
{
    public MarkAsReadCommandValidator()
    {
        RuleFor(x => x.NotificationId)
            .GreaterThan(0)
            .WithMessage("Notification ID must be greater than zero.");

        RuleFor(x => x.UpdatedByUserId)
            .GreaterThan(0)
            .WithMessage("UpdatedByUserId is required.");
    }
}
