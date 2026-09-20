using FluentAssertions;
using Hotel.Application.Rooming.TransferObjects;
using Hotel.IntegrationTests.Infrastructure;
using Hotel.IntegrationTests.Infrastructure.TestData;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Hotel.IntegrationTests.Rooming.Queries;

public class GetAvailableRoomsQueryTests : IClassFixture<HotelWebApplicationFactory>, IAsyncLifetime
{
    private readonly HotelWebApplicationFactory _factory;

    private HttpClient _client = null!;

    public GetAvailableRoomsQueryTests(HotelWebApplicationFactory factory)
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
    public async Task GetAvailableRooms_WithNoRatePlans_ReturnsEmptyList()
    {
        // Arrange
        var roomTypeId = await RoomTypeTestData.CreateRoomTypeAsync(_client);
        await RoomTestData.CreateRoomAsync(_client, "101", roomTypeId);

        var startDate = new DateOnly(2026, 1, 1);
        var endDate = new DateOnly(2026, 1, 5);

        // Act
        var response = await _client.GetAsync(
            $"/api/rooms/available?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var rooms = await response.Content.ReadFromJsonAsync<List<RoomListDto>>();
        rooms.Should().NotBeNull();
        rooms.Should().BeEmpty();
    }
}