using Application.CQRS.Notifications.Queries.Requests;
using FluentValidation;

namespace Application.Validators.Notifications;

public class GetUnreadNotificationsQueryValidator : AbstractValidator<GetUnreadNotificationsQuery>
{
    public GetUnreadNotificationsQueryValidator()
    {
        RuleFor(x => x.UserId).GreaterThan(0).WithMessage("UserId is required.");
    }
}
