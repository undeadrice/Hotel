using FluentAssertions;
using Hotel.Application.Rooming.Commands;
using Hotel.IntegrationTests.Infrastructure;
using Hotel.IntegrationTests.Infrastructure.TestData;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Hotel.IntegrationTests.Rooming.Commands;

public class CreateRoomCommandTests : IClassFixture<HotelWebApplicationFactory>, IAsyncLifetime
{
    private readonly HotelWebApplicationFactory _factory;

    private HttpClient _client = null!;

    public CreateRoomCommandTests(HotelWebApplicationFactory factory)
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
    public async Task CreateRoom_WithValidCommand_ReturnsRoomId()
    {
        // Arrange
        var roomTypeId = await RoomTypeTestData.CreateRoomTypeAsync(_client);

        // Act
        var roomId = await RoomTestData.CreateRoomAsync(_client, "101", roomTypeId);

        // Assert
        roomId.Should().NotBeEmpty();

        var getResponse = await _client.GetAsync($"/api/rooms/{roomId}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task CreateRoom_WithDuplicateRoomNumber_ReturnsBadRequest()
    {
        // Arrange
        var roomTypeId = await RoomTypeTestData.CreateRoomTypeAsync(_client);
        await RoomTestData.CreateRoomAsync(_client, "101", roomTypeId);

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/rooms",
            new CreateRoomCommand("101", roomTypeId));

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}