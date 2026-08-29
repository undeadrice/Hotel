using FluentAssertions;
using Hotel.Application.Rooming.Commands;
using Hotel.IntegrationTests.Infrastructure;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Hotel.IntegrationTests.Rooming.CommandValidations;

public class CreateRoomCommandValidationTests : IClassFixture<HotelWebApplicationFactory>, IAsyncLifetime
{
    private readonly HotelWebApplicationFactory _factory;

    private HttpClient _client = null!;

    public CreateRoomCommandValidationTests(HotelWebApplicationFactory factory)
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
    public async Task CreateRoom_WithEmptyRoomNumber_ReturnsBadRequest()
    {
        // Arrange
        var command = new CreateRoomCommand(string.Empty, Guid.NewGuid());

        // Act
        var response = await _client.PostAsJsonAsync("/api/rooms", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateRoom_WithRoomNumberExceedingMaxLength_ReturnsBadRequest()
    {
        // Arrange
        var command = new CreateRoomCommand(new string('A', 21), Guid.NewGuid());

        // Act
        var response = await _client.PostAsJsonAsync("/api/rooms", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateRoom_WithEmptyRoomTypeId_ReturnsBadRequest()
    {
        // Arrange
        var command = new CreateRoomCommand("101", Guid.Empty);

        // Act
        var response = await _client.PostAsJsonAsync("/api/rooms", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}