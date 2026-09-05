using FluentAssertions;
using Hotel.IntegrationTests.Infrastructure;
using System.Net;
using Xunit;

namespace Hotel.IntegrationTests.RatePlans.QueryValidations;

public class GetRatePlansByRoomTypeQueryValidationTests : IClassFixture<HotelWebApplicationFactory>, IAsyncLifetime
{
    private readonly HotelWebApplicationFactory _factory;

    private HttpClient _client = null!;

    public GetRatePlansByRoomTypeQueryValidationTests(HotelWebApplicationFactory factory)
    {
        _factory = factory;
    }

    public async Task InitializeAsync()
    {
        await _factory.CreateDatabase();
        _client = await _factory.CreateAuthenticatedClientAsync();
    }

    public async Task DisposeAsync()
    {
        await _factory.DeleteDatabase();
    }

    [Fact]
    public async Task GetRatePlansByRoom_WithEmptyRoomId_ReturnsBadRequest()
    {
        // Arrange
        var startDate = new DateOnly(2026, 1, 1);
        var endDate = new DateOnly(2026, 1, 5);

        // Act
        var response = await _client.GetAsync(
            $"/api/rateplans/by-room/{Guid.Empty}?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetRatePlansByRoom_WithStartDateAfterEndDate_ReturnsBadRequest()
    {
        // Arrange
        var roomId = Guid.NewGuid();
        var startDate = new DateOnly(2026, 1, 5);
        var endDate = new DateOnly(2026, 1, 1);

        // Act
        var response = await _client.GetAsync(
            $"/api/rateplans/by-room/{roomId}?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetRatePlansByRoom_WithMissingStartDate_ReturnsBadRequest()
    {
        // Arrange
        var roomId = Guid.NewGuid();
        var endDate = new DateOnly(2026, 1, 5);

        // Act
        var response = await _client.GetAsync(
            $"/api/rateplans/by-room/{roomId}?endDate={endDate:yyyy-MM-dd}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetRatePlansByRoom_WithMissingEndDate_ReturnsBadRequest()
    {
        // Arrange
        var roomId = Guid.NewGuid();
        var startDate = new DateOnly(2026, 1, 1);

        // Act
        var response = await _client.GetAsync(
            $"/api/rateplans/by-room/{roomId}?startDate={startDate:yyyy-MM-dd}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}