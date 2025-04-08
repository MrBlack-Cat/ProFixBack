using Application.CQRS.PortfolioItems.Commands.Handlers;
using Application.CQRS.PortfolioItems.Commands.Requests;
using Common.GlobalResponse;
using Domain.Entities;
using FluentAssertions;
using Moq;
using Repository.Common;

namespace ProFix.Tests.Handlers.PortfolioItems;

public class DeletePortfolioItemCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldSoftDelete_WhenItemExists()
    {
        // Arrange
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var fakeItem = new PortfolioItem
        {
            Id = 1,
            Title = "Test",
            Description = "To Delete",
            IsDeleted = false
        };

        mockUnitOfWork.Setup(x => x.PortfolioItemRepository.GetByIdAsync(1))
            .ReturnsAsync(fakeItem);

        var handler = new DeletePortfolioItemCommandHandler(mockUnitOfWork.Object);
        var command = new DeletePortfolioItemCommand(1, DeletedBy: 10, DeleteReason: "No longer needed");

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().Be("Portfolio item deleted successfully");

        fakeItem.IsDeleted.Should().BeTrue();
        fakeItem.DeletedBy.Should().Be(10);
        fakeItem.DeletedReason.Should().Be("No longer needed");
        fakeItem.DeletedAt.Should().NotBeNull();

        mockUnitOfWork.Verify(x => x.PortfolioItemRepository.DeleteAsync(fakeItem), Times.Once);
    }
}
