using Application.CQRS.Complaints.DTOs;
using FluentValidation;

namespace Application.Validators.Complaints
{
    public class CreateComplaintDtoValidator : AbstractValidator<CreateComplaintDto>
    {
        public CreateComplaintDtoValidator()
        {
            RuleFor(x => x.ToUserId)
                .NotEmpty().WithMessage("ToUserId is required.");

            RuleFor(x => x.TypeId)
                .NotEmpty().WithMessage("Complaint TypeId is required.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description must be less than 500 characters.");
        }
    }
}
