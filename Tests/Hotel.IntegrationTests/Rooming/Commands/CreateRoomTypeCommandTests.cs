using FluentAssertions;
using Hotel.Application.Rooming.Commands;
using Hotel.IntegrationTests.Infrastructure;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Hotel.IntegrationTests.Rooming.Commands;

public class CreateRoomTypeCommandTests : IClassFixture<HotelWebApplicationFactory>, IAsyncLifetime
{
    private readonly HotelWebApplicationFactory _factory;

    private HttpClient _client = null!;

    public CreateRoomTypeCommandTests(HotelWebApplicationFactory factory)
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
    public async Task CreateRoomType_WithValidCommand_ReturnsRoomTypeId()
    {
        // Arrange
        var command = new CreateRoomTypeCommand("Suite", "Luxury suite");

        // Act
        var response = await _client.PostAsJsonAsync("/api/roomtypes", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var roomTypeId = await response.Content.ReadFromJsonAsync<Guid>();
        roomTypeId.Should().NotBeEmpty();
    }
}