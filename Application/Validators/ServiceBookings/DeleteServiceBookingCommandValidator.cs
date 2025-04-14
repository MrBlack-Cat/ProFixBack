using Application.CQRS.ServiceBookings.Commands.Delete;
using FluentValidation;

namespace Application.Validators.ServiceBookings;

public class DeleteServiceBookingCommandValidator : AbstractValidator<DeleteServiceBookingCommandRequest>
{
    public DeleteServiceBookingCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Id is required.");

        RuleFor(x => x.DeletedReason)
            .NotEmpty().WithMessage("Delete reason is required.")
            .MaximumLength(250);
    }
}
