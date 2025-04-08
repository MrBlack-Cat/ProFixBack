using Application.CQRS.GuaranteeDocuments.Queries.Requests;
using FluentValidation;

namespace Application.Validators.GuaranteeDocuments;

public class GetAllGuaranteesByClientIdQueryValidator : AbstractValidator<GetAllGuaranteesByClientIdQuery>
{
    public GetAllGuaranteesByClientIdQueryValidator()
    {
        RuleFor(x => x.ClientProfileId)
            .GreaterThan(0)
            .WithMessage("ClientProfileId must be greater than 0.");
    }
}
