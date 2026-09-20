using FluentAssertions;
using Hotel.Application.Guests.Commands;
using Hotel.IntegrationTests.Infrastructure;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Hotel.IntegrationTests.Guests.Commands;

public class CreateGuestCommandTests : IClassFixture<HotelWebApplicationFactory>, IAsyncLifetime
{
    private readonly HotelWebApplicationFactory _factory;

    private HttpClient _client = null!;

    public CreateGuestCommandTests(HotelWebApplicationFactory factory)
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
    public async Task CreateGuest_WithValidCommand_ReturnsGuestId()
    {
        // Arrange
        var command = new CreateGuestCommand("John", "Doe", "123456789", "john.doe@example.com", "ABC123");

        // Act
        var response = await _client.PostAsJsonAsync("/api/guests", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var guestId = await response.Content.ReadFromJsonAsync<Guid>();
        guestId.Should().NotBeEmpty();
    }
}