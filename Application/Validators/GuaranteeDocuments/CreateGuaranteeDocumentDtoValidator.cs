using Application.CQRS.GuaranteeDocuments.DTOs;
using FluentValidation;

namespace Application.Validators.GuaranteeDocuments;

public class CreateGuaranteeDocumentDtoValidator : AbstractValidator<CreateGuaranteeDocumentDto>
{
    public CreateGuaranteeDocumentDtoValidator()
    {
        RuleFor(x => x.ClientProfileId)
            .GreaterThan(0)
            .WithMessage("ClientProfileId is required.");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .MaximumLength(1000);

        RuleFor(x => x.FileUrl)
            .NotEmpty().WithMessage("Document URL is required.")
            .MaximumLength(500);
    }
}
