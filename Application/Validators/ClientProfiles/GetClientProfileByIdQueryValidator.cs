using Application.CQRS.ClientProfiles.Queries.Requests;
using FluentValidation;

namespace Application.Validators.ClientProfiles;

public class GetClientProfileByIdQueryValidator : AbstractValidator<GetClientProfileByIdQuery>
{
    public GetClientProfileByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("ClientProfile ID must be greater than 0.");
    }
}
