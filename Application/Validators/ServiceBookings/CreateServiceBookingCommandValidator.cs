using Application.CQRS.ServiceBookings.Commands.Create;
using FluentValidation;

namespace Application.Validators.ServiceBookings;

public class CreateServiceBookingCommandValidator : AbstractValidator<CreateServiceBookingCommandRequest>
{
    public CreateServiceBookingCommandValidator()
    {
        RuleFor(x => x.Dto.ServiceProviderProfileId)
            .GreaterThan(0).WithMessage("ServiceProviderProfileId is required.");

        RuleFor(x => x.Dto.ScheduledDate)
            .GreaterThanOrEqualTo(DateTime.UtcNow.Date)
            .WithMessage("Date cannot be in the past.");

        RuleFor(x => x.Dto.EndTime)
            .GreaterThan(x => x.Dto.StartTime)
            .WithMessage("EndTime must be after StartTime.");
    }
}
