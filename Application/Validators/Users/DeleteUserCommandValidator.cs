using Application.CQRS.Users.Handlers;
using FluentValidation;

namespace Application.Validators.Users;

public class DeleteUserCommandValidator : AbstractValidator<DeleteHandler.Command>
{
    public DeleteUserCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.DeletedBy).GreaterThan(0);
        RuleFor(x => x.DeletedReason)
            .NotEmpty()
            .MaximumLength(1000);
    }
}
