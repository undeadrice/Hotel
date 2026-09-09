using FluentAssertions;
using Hotel.Application.Guests.Commands;
using Hotel.Application.Guests.TransferObjects;
using Hotel.IntegrationTests.Infrastructure;
using Hotel.IntegrationTests.Infrastructure.TestData;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Hotel.IntegrationTests.Guests.Commands;

public class UpdateGuestCommandTests : IClassFixture<HotelWebApplicationFactory>, IAsyncLifetime
{
    private readonly HotelWebApplicationFactory _factory;

    private HttpClient _client = null!;

    public UpdateGuestCommandTests(HotelWebApplicationFactory factory)
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
    public async Task UpdateGuest_WithValidCommand_ReturnsNoContent()
    {
        // Arrange
        var guestId = await GuestTestData.CreateGuestAsync(_client, "John", "Doe");
        var command = new UpdateGuestCommand(guestId, "Jane", "Smith", "987654321", "jane.smith@example.com", "XYZ789");

        // Act
        var response = await _client.PutAsJsonAsync("/api/guests/update", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var getResponse = await _client.GetAsync($"/api/guests/{guestId}");
        var guest = await getResponse.Content.ReadFromJsonAsync<GuestDto>();
        guest.Should().NotBeNull();
        guest!.FirstName.Should().Be("Jane");
        guest.LastName.Should().Be("Smith");
        guest.Phone.Should().Be("987654321");
        guest.Email.Should().Be("jane.smith@example.com");
        guest.DocumentNumber.Should().Be("XYZ789");
    }

    [Fact]
    public async Task UpdateGuest_WithNonExistentId_ReturnsNotFound()
    {
        // Arrange
        var command = new UpdateGuestCommand(Guid.NewGuid(), "John", "Doe", "123456789", "john.doe@example.com", "ABC123");

        // Act
        var response = await _client.PutAsJsonAsync("/api/guests/update", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}