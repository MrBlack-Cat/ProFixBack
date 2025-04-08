using Application.CQRS.GuaranteeDocuments.Queries.Requests;
using FluentValidation;

namespace Application.Validators.GuaranteeDocuments;

public class GetGuaranteeDocumentByIdQueryValidator : AbstractValidator<GetGuaranteeDocumentByIdQuery>
{
    public GetGuaranteeDocumentByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("GuaranteeDocument ID must be greater than 0.");
    }
}
