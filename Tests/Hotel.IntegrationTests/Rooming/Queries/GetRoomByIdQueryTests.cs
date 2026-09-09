using FluentAssertions;
using Hotel.Application.Rooming.TransferObjects;
using Hotel.IntegrationTests.Infrastructure;
using Hotel.IntegrationTests.Infrastructure.TestData;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Hotel.IntegrationTests.Rooming.Queries;

public class GetRoomByIdQueryTests : IClassFixture<HotelWebApplicationFactory>, IAsyncLifetime
{
    private readonly HotelWebApplicationFactory _factory;

    private HttpClient _client = null!;

    public GetRoomByIdQueryTests(HotelWebApplicationFactory factory)
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
    public async Task GetRoomById_WithExistingRoom_ReturnsRoom()
    {
        // Arrange
        var roomTypeId = await RoomTypeTestData.CreateRoomTypeAsync(_client, "Suite", "Luxury suite");
        var roomId = await RoomTestData.CreateRoomAsync(_client, "101", roomTypeId);

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
}