using FluentAssertions;
using Hotel.Application.Rooming.Commands;
using Hotel.IntegrationTests.Infrastructure;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Hotel.IntegrationTests.Rooming.CommandValidations;

public class UpdateRoomTypeCommandValidationTests : IClassFixture<HotelWebApplicationFactory>, IAsyncLifetime
{
    private readonly HotelWebApplicationFactory _factory;

    private HttpClient _client = null!;

    public UpdateRoomTypeCommandValidationTests(HotelWebApplicationFactory factory)
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
    public async Task UpdateRoomType_WithEmptyId_ReturnsBadRequest()
    {
        // Arrange
        var command = new UpdateRoomTypeCommand(Guid.Empty, "Standard", null);

        // Act
        var response = await _client.PutAsJsonAsync("/api/rooms/types", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateRoomType_WithEmptyName_ReturnsBadRequest()
    {
        // Arrange
        var command = new UpdateRoomTypeCommand(Guid.NewGuid(), string.Empty, null);

        // Act
        var response = await _client.PutAsJsonAsync("/api/rooms/types", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateRoomType_WithNameExceedingMaxLength_ReturnsBadRequest()
    {
        // Arrange
        var command = new UpdateRoomTypeCommand(Guid.NewGuid(), new string('A', 101), null);

        // Act
        var response = await _client.PutAsJsonAsync("/api/rooms/types", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateRoomType_WithDescriptionExceedingMaxLength_ReturnsBadRequest()
    {
        // Arrange
        var command = new UpdateRoomTypeCommand(Guid.NewGuid(), "Standard", new string('A', 501));

        // Act
        var response = await _client.PutAsJsonAsync("/api/rooms/types", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}