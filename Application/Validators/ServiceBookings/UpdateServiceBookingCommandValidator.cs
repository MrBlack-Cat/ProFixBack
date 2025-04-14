using Application.CQRS.ServiceBookings.Commands.Update;
using FluentValidation;

namespace Application.Validators.ServiceBookings;

public class UpdateServiceBookingCommandValidator : AbstractValidator<UpdateServiceBookingCommandRequest>
{
    public UpdateServiceBookingCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Id is required.");

        When(x => x.Dto.ScheduledDate.HasValue, () =>
        {
            RuleFor(x => x.Dto.ScheduledDate.Value)
                .GreaterThanOrEqualTo(DateTime.UtcNow.Date)
                .WithMessage("Cannot update to a past date.");
        });

        When(x => x.Dto.StartTime.HasValue && x.Dto.EndTime.HasValue, () =>
        {
            RuleFor(x => x.Dto.EndTime.Value)
                .GreaterThan(x => x.Dto.StartTime.Value)
                .WithMessage("EndTime must be after StartTime.");
        });
    }
}
