using Application.CQRS.ClientProfiles.Commands.Requests;
using FluentValidation;

namespace Application.Validators.ClientProfile;

public class DeleteClientProfileCommandValidator : AbstractValidator<DeleteClientProfileCommand>
{
    public DeleteClientProfileCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Profile ID must be greater than zero.");

        RuleFor(x => x.DeletedBy)
            .GreaterThan(0)
            .WithMessage("DeletedBy (ID of the user performing the deletion) is required.");

        RuleFor(x => x.DeletedReason)
            .NotEmpty()
            .WithMessage("Deletion reason is required.")
            .MaximumLength(1000)
            .WithMessage("Deletion reason must not exceed 1000 characters.");
    }
}
