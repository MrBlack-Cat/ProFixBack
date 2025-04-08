using Application.CQRS.Notifications.Queries.Requests;
using FluentValidation;

namespace Application.Validators.Notifications;

public class GetNotificationByIdQueryValidator : AbstractValidator<GetNotificationByIdQuery>
{
    public GetNotificationByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Notification ID must be greater than 0.");
    }
}
