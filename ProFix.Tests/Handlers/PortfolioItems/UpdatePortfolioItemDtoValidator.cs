using Application.CQRS.PortfolioItems.DTOs;
using Application.Validators.PortfolioItems;
using FluentValidation.TestHelper;

namespace ProFix.Tests.Validators.PortfolioItems;

public class UpdatePortfolioItemDtoValidatorTests
{
    private readonly UpdatePortfolioItemDtoValidator _validator = new();

    [Fact]
    public void Should_HaveValidationErrors_WhenFieldsAreEmpty()
    {
        var dto = new UpdatePortfolioItemDto();
        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Title);
        result.ShouldHaveValidationErrorFor(x => x.Description);
        result.ShouldHaveValidationErrorFor(x => x.ImageUrl);
    }

    [Fact]
    public void Should_NotHaveErrors_WhenFieldsValid()
    {
        var dto = new UpdatePortfolioItemDto
        {
            Title = "Updated",
            Description = "Updated desc",
            ImageUrl = "https://test.com/image.jpg"
        };

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveAnyValidationErrors();
    }
}
