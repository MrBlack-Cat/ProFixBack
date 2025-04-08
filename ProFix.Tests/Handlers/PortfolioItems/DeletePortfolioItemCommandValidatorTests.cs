using Application.CQRS.PortfolioItems.Commands.Requests;
using Application.Validators.PortfolioItems;
using FluentValidation.TestHelper;

namespace ProFix.Tests.Validators.PortfolioItems;

public class DeletePortfolioItemCommandValidatorTests
{
    private readonly DeletePortfolioItemCommandValidator _validator = new();

    [Fact]
    public void Should_HaveError_When_ReasonIsEmpty()
    {
        var command = new DeletePortfolioItemCommand(1, 2, "");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.DeleteReason);
    }

    [Fact]
    public void Should_HaveNoError_When_Valid()
    {
        var command = new DeletePortfolioItemCommand(1, 2, "User removed this item");
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
