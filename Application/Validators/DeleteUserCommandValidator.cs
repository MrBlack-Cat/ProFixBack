//using Application.CQRS.Users.Commands.Requests;
//using FluentValidation;

//namespace Application.Validators.Users;

//public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
//{
//    public DeleteUserCommandValidator()
//    {
//        RuleFor(x => x.Id)
//            .GreaterThan(0).WithMessage("User ID is required");

//        RuleFor(x => x.DeletedByUserId)
//            .GreaterThan(0).WithMessage("DeletedByUserId must be provided");

//        RuleFor(x => x.Reason)
//            .MaximumLength(250).WithMessage("Delete reason is too long");
//    }
//}
