using FluentAssertions;
using Hotel.Application.Rooming.Commands;
using Hotel.IntegrationTests.Infrastructure;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Hotel.IntegrationTests.Rooming.CommandValidations;

public class UpdateRoomCommandValidationTests : IClassFixture<HotelWebApplicationFactory>, IAsyncLifetime
{
    private readonly HotelWebApplicationFactory _factory;

    private HttpClient _client = null!;

    public UpdateRoomCommandValidationTests(HotelWebApplicationFactory factory)
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
    public async Task UpdateRoom_WithEmptyId_ReturnsBadRequest()
    {
        // Arrange
        var command = new UpdateRoomCommand(Guid.Empty, "101", Guid.NewGuid());

        // Act
        var response = await _client.PutAsJsonAsync("/api/rooms", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateRoom_WithEmptyRoomNumber_ReturnsBadRequest()
    {
        // Arrange
        var command = new UpdateRoomCommand(Guid.NewGuid(), string.Empty, Guid.NewGuid());

        // Act
        var response = await _client.PutAsJsonAsync("/api/rooms", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateRoom_WithRoomNumberExceedingMaxLength_ReturnsBadRequest()
    {
        // Arrange
        var command = new UpdateRoomCommand(Guid.NewGuid(), new string('A', 21), Guid.NewGuid());

        // Act
        var response = await _client.PutAsJsonAsync("/api/rooms", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateRoom_WithEmptyRoomTypeId_ReturnsBadRequest()
    {
        // Arrange
        var command = new UpdateRoomCommand(Guid.NewGuid(), "101", Guid.Empty);

        // Act
        var response = await _client.PutAsJsonAsync("/api/rooms", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}