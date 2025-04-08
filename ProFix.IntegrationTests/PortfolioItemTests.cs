using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Application.CQRS.PortfolioItems.DTOs;
using Common.GlobalResponse;
using FluentAssertions;
using Newtonsoft.Json;
using ProFix.IntegrationTests.Helpers;
using Xunit;

namespace ProFix.IntegrationTests;

public class PortfolioItemTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public PortfolioItemTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetById_Should_Return_OK()
    {
        // Arrange: авторизация
        var token = await IntegrationTestHelper.GetJwtTokenAsync(_client, "Admin1@gmail.com", "гыук123");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Act
        var response = await _client.GetAsync("/api/portfolioitem/1");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<ResponseModel<GetPortfolioItemByIdDto>>(content);

        result.Should().NotBeNull();
        result!.IsSuccess.Should().BeTrue();
        result.Data.Title.Should().Be("Test");
    }




    public async Task GetAllByCurrentUser_ShouldReturnList()
    {
        var token = await IntegrationTestHelper.GetJwtTokenAsync(_client);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.GetAsync("/api/PortfolioItem/my");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("isSuccess");
        content.Should().Contain("data");
    }



    [Fact]
    public async Task CreatePortfolioItem_Should_Return_Success()
    {
        // Arrange: 
        var token = await IntegrationTestHelper.GetJwtTokenAsync(_client);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var dto = new CreatePortfolioItemDto
        {
            Title = "Test Portfolio",
            Description = "Integration test",
            ImageUrl = "test.jpg"
        };

        var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/PortfolioItem", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseBody = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<ResponseModel<GetPortfolioItemByIdDto>>(responseBody);

        result.Should().NotBeNull();
        result!.IsSuccess.Should().BeTrue();
        result.Data.Title.Should().Be(dto.Title);
        result.Data.Description.Should().Be(dto.Description);
    }
}
