using Application.CQRS.ClientProfiles.Commands.Requests;
using FluentValidation;

namespace Application.Validators.ClientProfiles;

public class CreateClientProfileCommandValidator : AbstractValidator<CreateClientProfileCommand>
{
    public CreateClientProfileCommandValidator()
    {
        RuleFor(x => x.UserId).GreaterThan(0);
        RuleFor(x => x.Profile.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Profile.Surname).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Profile.City).MaximumLength(100);
        RuleFor(x => x.Profile.AvatarUrl).MaximumLength(500);
        RuleFor(x => x.Profile.About).MaximumLength(1000);
        RuleFor(x => x.Profile.OtherContactLinks).MaximumLength(1000);
    }
}
