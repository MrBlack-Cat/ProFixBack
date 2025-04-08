using Application.CQRS.PortfolioItems.Commands.Handlers;
using Application.CQRS.PortfolioItems.Commands.Requests;
using Application.CQRS.PortfolioItems.DTOs;
using AutoMapper;
using Domain.Entities;
using FluentAssertions;
using Moq;
using Repository.Common;

namespace ProFix.Tests.Handlers.PortfolioItems;

public class CreatePortfolioItemCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenProfileExists()
    {
        // Arrange
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var mockMapper = new Mock<IMapper>();

        mockUnitOfWork.Setup(x => x.ServiceProviderProfileRepository.GetByUserIdAsync(1))
            .ReturnsAsync(new ServiceProviderProfile { Id = 77 });

        // 👇 Вот это важно — иначе будет NullReferenceException!
        mockUnitOfWork.Setup(x => x.PortfolioItemRepository.AddAsync(It.IsAny<PortfolioItem>()))
            .Returns(Task.CompletedTask);

        var handler = new CreatePortfolioItemCommandHandler(mockUnitOfWork.Object, mockMapper.Object);

        var dto = new CreatePortfolioItemDto
        {
            Title = "Test Portfolio",
            Description = "Test Description",
            ImageUrl = "https://test.com/image.jpg"
        };

        var command = new CreatePortfolioItemCommand(dto, CurrentUserId: 1);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().Be("Portfolio item created successfully");

        mockUnitOfWork.Verify(x => x.PortfolioItemRepository.AddAsync(It.IsAny<PortfolioItem>()), Times.Once);
    }

}
