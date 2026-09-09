using FluentAssertions;
using Hotel.Application.Rooming.Commands;
using Hotel.Application.Rooming.TransferObjects;
using Hotel.IntegrationTests.Infrastructure;
using Hotel.IntegrationTests.Infrastructure.TestData;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Hotel.IntegrationTests.Rooming.Commands;

public class UpdateRoomTypeCommandTests : IClassFixture<HotelWebApplicationFactory>, IAsyncLifetime
{
    private readonly HotelWebApplicationFactory _factory;

    private HttpClient _client = null!;

    public UpdateRoomTypeCommandTests(HotelWebApplicationFactory factory)
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
    public async Task UpdateRoomType_WithValidCommand_ReturnsNoContent()
    {
        // Arrange
        var roomTypeId = await RoomTypeTestData.CreateRoomTypeAsync(_client, "Standard", "Original");
        var command = new UpdateRoomTypeCommand(roomTypeId, "Deluxe", "Updated");

        // Act
        var response = await _client.PutAsJsonAsync("/api/roomtypes", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var getResponse = await _client.GetAsync($"/api/roomtypes/{roomTypeId}");
        var roomType = await getResponse.Content.ReadFromJsonAsync<RoomTypeDto>();
        roomType.Should().NotBeNull();
        roomType!.Name.Should().Be("Deluxe");
        roomType.Description.Should().Be("Updated");
    }

    [Fact]
    public async Task UpdateRoomType_WithNonExistentId_ReturnsNotFound()
    {
        // Arrange
        var command = new UpdateRoomTypeCommand(Guid.NewGuid(), "Standard", null);

        // Act
        var response = await _client.PutAsJsonAsync("/api/roomtypes", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}