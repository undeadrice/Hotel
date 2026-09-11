using FluentAssertions;
using Hotel.Application.Rooming.Commands;
using Hotel.Application.Rooming.TransferObjects;
using Hotel.IntegrationTests.Infrastructure;
using Hotel.IntegrationTests.Infrastructure.TestData;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Hotel.IntegrationTests.Rooming.Commands;

public class UpdateRoomCommandTests : IClassFixture<HotelWebApplicationFactory>, IAsyncLifetime
{
    private readonly HotelWebApplicationFactory _factory;

    private HttpClient _client = null!;

    public UpdateRoomCommandTests(HotelWebApplicationFactory factory)
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
    public async Task UpdateRoom_WithValidCommand_ReturnsNoContent()
    {
        // Arrange
        var roomTypeId = await RoomTypeTestData.CreateRoomTypeAsync(_client, "Suite", "Luxury suite");
        var newRoomTypeId = await RoomTypeTestData.CreateRoomTypeAsync(_client, "Double", "Double room");
        var roomId = await RoomTestData.CreateRoomAsync(_client, "101", roomTypeId);

        var command = new UpdateRoomCommand(roomId, "102", newRoomTypeId);

        // Act
        var response = await _client.PutAsJsonAsync("/api/rooms", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var getResponse = await _client.GetAsync($"/api/rooms/{roomId}");
        var room = await getResponse.Content.ReadFromJsonAsync<RoomDto>();
        room.Should().NotBeNull();
        room!.RoomNumber.Should().Be("102");
        room.RoomTypeId.Should().Be(newRoomTypeId);
    }

    [Fact]
    public async Task UpdateRoom_WithNonExistentId_ReturnsNotFound()
    {
        // Arrange
        var roomTypeId = await RoomTypeTestData.CreateRoomTypeAsync(_client);
        var command = new UpdateRoomCommand(Guid.NewGuid(), "101", roomTypeId);

        // Act
        var response = await _client.PutAsJsonAsync("/api/rooms", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateRoom_WithDuplicateRoomNumber_ReturnsBadRequest()
    {
        // Arrange
        var roomTypeId = await RoomTypeTestData.CreateRoomTypeAsync(_client);
        await RoomTestData.CreateRoomAsync(_client, "101", roomTypeId);
        var roomId = await RoomTestData.CreateRoomAsync(_client, "102", roomTypeId);

        var command = new UpdateRoomCommand(roomId, "101", roomTypeId);

        // Act
        var response = await _client.PutAsJsonAsync("/api/rooms", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}