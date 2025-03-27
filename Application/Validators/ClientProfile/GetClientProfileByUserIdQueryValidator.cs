using Application.CQRS.ClientProfiles.Queries.Requests;
using FluentValidation;

namespace Application.Validators.ClientProfiles;

public class GetClientProfileByUserIdQueryValidator : AbstractValidator<GetClientProfileByUserIdQuery>
{
    public GetClientProfileByUserIdQueryValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithMessage("UserId must be greater than 0.");
    }
}
