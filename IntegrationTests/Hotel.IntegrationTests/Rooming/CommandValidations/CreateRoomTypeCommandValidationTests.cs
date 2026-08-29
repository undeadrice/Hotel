using FluentAssertions;
using Hotel.Application.Rooming.Commands;
using Hotel.IntegrationTests.Infrastructure;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Hotel.IntegrationTests.Rooming.CommandValidations;

public class CreateRoomTypeCommandValidationTests : IClassFixture<HotelWebApplicationFactory>, IAsyncLifetime
{
    private readonly HotelWebApplicationFactory _factory;

    private HttpClient _client = null!;

    public CreateRoomTypeCommandValidationTests(HotelWebApplicationFactory factory)
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
    public async Task CreateRoomType_WithEmptyName_ReturnsBadRequest()
    {
        // Arrange
        var command = new CreateRoomTypeCommand(string.Empty, null);

        // Act
        var response = await _client.PostAsJsonAsync("/api/roomtypes", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateRoomType_WithNameExceedingMaxLength_ReturnsBadRequest()
    {
        // Arrange
        var command = new CreateRoomTypeCommand(new string('A', 101), null);

        // Act
        var response = await _client.PostAsJsonAsync("/api/roomtypes", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateRoomType_WithDescriptionExceedingMaxLength_ReturnsBadRequest()
    {
        // Arrange
        var command = new CreateRoomTypeCommand("Standard", new string('A', 501));

        // Act
        var response = await _client.PostAsJsonAsync("/api/roomtypes", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}