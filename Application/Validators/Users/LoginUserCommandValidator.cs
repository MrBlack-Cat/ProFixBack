using Application.CQRS.Users.Handlers;
using FluentValidation;

namespace Application.Validators.Users;

public class LoginUserCommandValidator : AbstractValidator<LoginHandlers.LoginRequest>
{
    public LoginUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.");
    }
}
