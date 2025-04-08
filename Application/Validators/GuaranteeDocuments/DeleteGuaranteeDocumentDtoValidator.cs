using Application.CQRS.GuaranteeDocuments.DTOs;
using FluentValidation;

namespace Application.Validators.GuaranteeDocuments;

public class DeleteGuaranteeDocumentDtoValidator : AbstractValidator<DeleteGuaranteeDocumentDto>
{
    public DeleteGuaranteeDocumentDtoValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Guarantee Document ID is required.");

        RuleFor(x => x.DeletedByUserId)
            .NotEmpty().WithMessage("DeletedByUserId is required.");

        RuleFor(x => x.Reason)
            .NotEmpty().WithMessage("Reason is required.")
            .MaximumLength(500);
    }
}
