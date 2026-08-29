using FluentAssertions;
using Hotel.Application.Rooming.Commands;
using Hotel.IntegrationTests.Infrastructure;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Hotel.IntegrationTests.Rooming.CommandValidations;

public class DeactivateRoomCommandValidationTests : IClassFixture<HotelWebApplicationFactory>, IAsyncLifetime
{
    private readonly HotelWebApplicationFactory _factory;

    private HttpClient _client = null!;

    public DeactivateRoomCommandValidationTests(HotelWebApplicationFactory factory)
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
    public async Task DeactivateRoom_WithEmptyRoomId_ReturnsBadRequest()
    {
        // Arrange
        var command = new DeactivateRoomCommand(Guid.Empty);

        // Act
        var response = await _client.PutAsJsonAsync("/api/rooms/deactivate", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}