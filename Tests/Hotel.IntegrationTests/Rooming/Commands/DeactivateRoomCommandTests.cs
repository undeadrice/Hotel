using FluentAssertions;
using Hotel.Application.Rooming.Commands;
using Hotel.Application.Rooming.TransferObjects;
using Hotel.IntegrationTests.Infrastructure;
using Hotel.IntegrationTests.Infrastructure.TestData;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Hotel.IntegrationTests.Rooming.Commands;

public class DeactivateRoomCommandTests : IClassFixture<HotelWebApplicationFactory>, IAsyncLifetime
{
    private readonly HotelWebApplicationFactory _factory;

    private HttpClient _client = null!;

    public DeactivateRoomCommandTests(HotelWebApplicationFactory factory)
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
    public async Task DeactivateRoom_WithValidCommand_ReturnsNoContent()
    {
        // Arrange
        var roomTypeId = await RoomTypeTestData.CreateRoomTypeAsync(_client);
        var roomId = await RoomTestData.CreateRoomAsync(_client, "101", roomTypeId);

        var command = new DeactivateRoomCommand(roomId);

        // Act
        var response = await _client.PutAsJsonAsync("/api/rooms/deactivate", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var getResponse = await _client.GetAsync($"/api/rooms/{roomId}");
        var room = await getResponse.Content.ReadFromJsonAsync<RoomDto>();
        room.Should().NotBeNull();
        room!.IsActive.Should().BeFalse();
    }

    [Fact]
    public async Task DeactivateRoom_WithNonExistentId_ReturnsNotFound()
    {
        // Arrange
        var command = new DeactivateRoomCommand(Guid.NewGuid());

        // Act
        var response = await _client.PutAsJsonAsync("/api/rooms/deactivate", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeactivateRoom_WhenAlreadyDeactivated_ReturnsBadRequest()
    {
        // Arrange
        var roomTypeId = await RoomTypeTestData.CreateRoomTypeAsync(_client);
        var roomId = await RoomTestData.CreateRoomAsync(_client, "101", roomTypeId);

        var command = new DeactivateRoomCommand(roomId);
        var firstResponse = await _client.PutAsJsonAsync("/api/rooms/deactivate", command);
        firstResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Act
        var response = await _client.PutAsJsonAsync("/api/rooms/deactivate", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}