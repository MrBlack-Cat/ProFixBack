using Application.CQRS.PortfolioItems.Queries.Handlers;
using Application.CQRS.PortfolioItems.Queries.Requests;
using Application.CQRS.PortfolioItems.DTOs;
using AutoMapper;
using Domain.Entities;
using FluentAssertions;
using Moq;
using Repository.Common;

namespace ProFix.Tests.Handlers.PortfolioItems;

public class GetPortfolioItemByIdQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnDto_WhenItemExists()
    {
        // Arrange
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var mockMapper = new Mock<IMapper>();

        var entity = new PortfolioItem
        {
            Id = 1,
            Title = "Title",
            Description = "Desc",
            ImageUrl = "url.jpg"
        };

        var dto = new GetPortfolioItemByIdDto
        {
            Id = 1,
            Title = "Title",
            Description = "Desc",
            ImageUrl = "url.jpg"
        };

        mockUnitOfWork.Setup(x => x.PortfolioItemRepository.GetByIdAsync(1)).ReturnsAsync(entity);
        mockMapper.Setup(x => x.Map<GetPortfolioItemByIdDto>(entity)).Returns(dto);

        var handler = new GetPortfolioItemByIdQueryHandler(mockUnitOfWork.Object, mockMapper.Object);
        var query = new GetPortfolioItemByIdQuery(1);

        // Act
        var result = await handler.Handle(query, default);

        // Assert
        result.Data.Should().BeEquivalentTo(dto);
        result.IsSuccess.Should().BeTrue();
    }
}
