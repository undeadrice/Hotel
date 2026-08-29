using FluentAssertions;
using Hotel.Application.Rooming.TransferObjects;
using Hotel.IntegrationTests.Infrastructure;
using Hotel.IntegrationTests.Infrastructure.TestData;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Hotel.IntegrationTests.Rooming.Queries;

public class RoomQueryTests : IClassFixture<HotelWebApplicationFactory>, IAsyncLifetime
{
    private readonly HotelWebApplicationFactory _factory;

    private HttpClient _client = null!;

    public RoomQueryTests(HotelWebApplicationFactory factory)
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
    public async Task GetRooms_WhenNoRoomsExist_ReturnsEmptyList()
    {
        // Act
        var response = await _client.GetAsync("/api/rooms");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var rooms = await response.Content.ReadFromJsonAsync<List<RoomListDto>>();
        rooms.Should().NotBeNull();
        rooms.Should().BeEmpty();
    }

    [Fact]
    public async Task GetRooms_AfterCreatingRoom_ReturnsRoom()
    {
        // Arrange
        var roomTypeId = await RoomTypeTestData.CreateRoomTypeAsync(_client, "Suite", "Luxury suite");
        await RoomTestData.CreateRoomAsync(_client, "101", roomTypeId);

        // Act
        var response = await _client.GetAsync("/api/rooms");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var rooms = await response.Content.ReadFromJsonAsync<List<RoomListDto>>();
        rooms.Should().ContainSingle(r => r.RoomNumber == "101");
        rooms!.Single(r => r.RoomNumber == "101").RoomType.Should().Be("Suite");
    }

    [Fact]
    public async Task GetRoomById_WithExistingRoom_ReturnsRoom()
    {
        // Arrange
        var roomTypeId = await RoomTypeTestData.CreateRoomTypeAsync(_client, "Suite", "Luxury suite");
        await RoomTestData.CreateRoomAsync(_client, "101", roomTypeId);
        var roomId = await RoomTestData.GetRoomIdAsync(_client, "101");

        // Act
        var response = await _client.GetAsync($"/api/rooms/{roomId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var room = await response.Content.ReadFromJsonAsync<RoomDto>();
        room.Should().NotBeNull();
        room!.Id.Should().Be(roomId);
        room.RoomNumber.Should().Be("101");
        room.RoomTypeId.Should().Be(roomTypeId);
        room.RoomTypeName.Should().Be("Suite");
        room.IsActive.Should().BeTrue();
    }

    [Fact]
    public async Task GetRoomById_WithNonExistentId_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync($"/api/rooms/{Guid.NewGuid()}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
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