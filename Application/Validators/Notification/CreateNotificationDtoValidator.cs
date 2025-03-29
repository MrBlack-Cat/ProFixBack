using Application.CQRS.Notifications.DTOs;
using FluentValidation;

public class CreateNotificationDtoValidator : AbstractValidator<CreateNotificationDto>
{
    public CreateNotificationDtoValidator()
    {
        RuleFor(x => x.UserId).GreaterThan(0);
        RuleFor(x => x.TypeId).GreaterThan(0);
        RuleFor(x => x.Message).NotEmpty().MaximumLength(1000);
    }
}
