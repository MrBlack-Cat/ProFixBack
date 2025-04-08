using Application.CQRS.GuaranteeDocuments.DTOs;
using FluentValidation;

namespace Application.Validators.GuaranteeDocuments;

public class UpdateGuaranteeDocumentDtoValidator : AbstractValidator<UpdateGuaranteeDocumentDto>
{
    public UpdateGuaranteeDocumentDtoValidator()
    {

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
