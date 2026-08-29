using FluentAssertions;
using Hotel.IntegrationTests.Infrastructure;
using System.Net;
using Xunit;

namespace Hotel.IntegrationTests.Rooming.QueryValidations;

public class GetAvailableRoomsQueryValidationTests : IClassFixture<HotelWebApplicationFactory>, IAsyncLifetime
{
    private readonly HotelWebApplicationFactory _factory;

    private HttpClient _client = null!;

    public GetAvailableRoomsQueryValidationTests(HotelWebApplicationFactory factory)
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
    public async Task GetAvailableRooms_WithStartDateAfterEndDate_ReturnsBadRequest()
    {
        // Arrange
        var startDate = new DateOnly(2026, 1, 2);
        var endDate = new DateOnly(2026, 1, 1);

        // Act
        var response = await _client.GetAsync($"/api/rooms/available?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetAvailableRooms_WithMissingEndDate_ReturnsBadRequest()
    {
        // Arrange
        var startDate = new DateOnly(2026, 1, 1);

        // Act
        var response = await _client.GetAsync($"/api/rooms/available?startDate={startDate:yyyy-MM-dd}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}