using Application.CQRS.ClientProfiles.Commands.Requests;
using FluentValidation;

namespace Application.Validators.ClientProfiles;

public class UpdateClientProfileCommandValidator : AbstractValidator<UpdateClientProfileCommand>
{
    public UpdateClientProfileCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("ClientProfile ID must be greater than 0.");

        RuleFor(x => x.UpdatedBy)
            .GreaterThan(0)
            .WithMessage("UpdatedBy (user ID) is required.");

        RuleFor(x => x.Dto.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100);

        RuleFor(x => x.Dto.Surname)
            .NotEmpty().WithMessage("Surname is required.")
            .MaximumLength(100);

        RuleFor(x => x.Dto.City)
            .MaximumLength(100);

        RuleFor(x => x.Dto.AvatarUrl)
            .MaximumLength(500);

        RuleFor(x => x.Dto.About)
            .MaximumLength(1000);

        RuleFor(x => x.Dto.OtherContactLinks)
            .MaximumLength(1000);
    }
}
