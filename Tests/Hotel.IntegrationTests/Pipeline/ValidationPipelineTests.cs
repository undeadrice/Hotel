using FluentAssertions;
using Hotel.Application.Rooming.Commands;
using Hotel.IntegrationTests.Infrastructure;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Hotel.IntegrationTests.Pipeline;

public class ValidationPipelineTests : IClassFixture<HotelWebApplicationFactory>, IAsyncLifetime
{
    private readonly HotelWebApplicationFactory _factory;

    private HttpClient _client = null!;

    public ValidationPipelineTests(HotelWebApplicationFactory factory)
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
}
