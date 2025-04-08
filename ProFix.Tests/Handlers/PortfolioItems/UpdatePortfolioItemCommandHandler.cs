using Application.CQRS.PortfolioItems.Commands.Handlers;
using Application.CQRS.PortfolioItems.Commands.Requests;
using Application.CQRS.PortfolioItems.DTOs;
using Domain.Entities;
using FluentAssertions;
using Moq;
using Repository.Common;

namespace ProFix.Tests.Handlers.PortfolioItems;

public class UpdatePortfolioItemCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldUpdateItem_WhenItemExists()
    {
        // Arrange
        var mockUnitOfWork = new Mock<IUnitOfWork>();

        var existingItem = new PortfolioItem
        {
            Id = 5,
            Title = "Old Title",
            Description = "Old Desc",
            ImageUrl = "old.jpg",
            IsDeleted = false
        };

        mockUnitOfWork.Setup(x => x.PortfolioItemRepository.GetByIdAsync(5))
            .ReturnsAsync(existingItem);

        var handler = new UpdatePortfolioItemCommandHandler(mockUnitOfWork.Object);

        var dto = new UpdatePortfolioItemDto
        {
            Title = "New Title",
            Description = "New Desc",
            ImageUrl = "new.jpg"
        };

        var command = new UpdatePortfolioItemCommand(Id: 5, CurrentUserId: 2, Dto: dto);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().Be("Portfolio item updated successfully");

        existingItem.Title.Should().Be("New Title");
        existingItem.Description.Should().Be("New Desc");
        existingItem.ImageUrl.Should().Be("new.jpg");
        existingItem.UpdatedBy.Should().Be(2);
        existingItem.UpdatedAt.Should().NotBeNull();

        mockUnitOfWork.Verify(x => x.PortfolioItemRepository.UpdateAsync(existingItem), Times.Once);
    }
}
