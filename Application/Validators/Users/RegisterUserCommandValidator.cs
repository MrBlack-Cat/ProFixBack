using Application.CQRS.Users.Handlers;
using FluentValidation;

namespace Application.Validators.Users;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserHandler.RegisterCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("Username is required.")
            .MaximumLength(100);

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6);

        RuleFor(x => x.RoleId)
            .GreaterThan(0).WithMessage("RoleId must be a positive integer.");
    }
}
