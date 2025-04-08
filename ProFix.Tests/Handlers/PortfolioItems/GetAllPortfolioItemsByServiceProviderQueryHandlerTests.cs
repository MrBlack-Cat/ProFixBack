using Application.CQRS.PortfolioItems.DTOs;
using Application.CQRS.PortfolioItems.Queries.Handlers;
using Application.CQRS.PortfolioItems.Queries.Requests;
using AutoMapper;
using Domain.Entities;
using FluentAssertions;
using Moq;
using Repository.Common;

namespace ProFix.Tests.Handlers.PortfolioItems;

public class GetAllPortfolioItemsByServiceProviderQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnList_WhenEverythingIsMockedProperly()
    {
        // Arrange
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var mapperMock = new Mock<IMapper>();

        var serviceProvider = new ServiceProviderProfile { Id = 99 };
        var portfolioItems = new List<PortfolioItem>
        {
            new() { Id = 1, Title = "Test 1", ImageUrl = "img1" },
            new() { Id = 2, Title = "Test 2", ImageUrl = "img2" }
        };
        var portfolioDtos = new List<PortfolioItemListDto>
        {
            new() { Id = 1, Title = "Test 1", ImageUrl = "img1" },
            new() { Id = 2, Title = "Test 2", ImageUrl = "img2" }
        };

        // Мокаем репозитории напрямую
        unitOfWorkMock.Setup(u => u.ServiceProviderProfileRepository.GetByUserIdAsync(It.IsAny<int>()))
            .ReturnsAsync(serviceProvider);

        unitOfWorkMock.Setup(u => u.PortfolioItemRepository.GetByServiceProviderIdAsync(serviceProvider.Id))
            .ReturnsAsync(portfolioItems);

        mapperMock.Setup(m => m.Map<List<PortfolioItemListDto>>(portfolioItems)).Returns(portfolioDtos);

        var handler = new GetAllPortfolioItemsByServiceProviderQueryHandler(unitOfWorkMock.Object, mapperMock.Object);
        var query = new GetAllPortfolioItemsByServiceProviderQuery(CurrentUserId: 1);

        // Act
        var result = await handler.Handle(query, default);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().HaveCount(2);
        result.Data[0].Title.Should().Be("Test 1");
    }
}
