using Application.CQRS.Notifications.Queries.Requests;
using FluentValidation;

namespace Application.Validators.Notifications;

public class GetAllNotificationsByUserIdQueryValidator : AbstractValidator<GetAllNotificationsByUserIdQuery>
{
    public GetAllNotificationsByUserIdQueryValidator()
    {
        RuleFor(x => x.UserId).GreaterThan(0).WithMessage("UserId is required.");
    }
}
