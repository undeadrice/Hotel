using FluentAssertions;
using Hotel.Application.Guests.TransferObjects;
using Hotel.IntegrationTests.Infrastructure;
using Hotel.IntegrationTests.Infrastructure.TestData;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Hotel.IntegrationTests.Guests.Queries;

public class GetGuestByIdQueryTests : IClassFixture<HotelWebApplicationFactory>, IAsyncLifetime
{
    private readonly HotelWebApplicationFactory _factory;

    private HttpClient _client = null!;

    public GetGuestByIdQueryTests(HotelWebApplicationFactory factory)
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
    public async Task GetGuestById_WithExistingGuest_ReturnsGuest()
    {
        // Arrange
        var guestId = await GuestTestData.CreateGuestAsync(
            _client,
            "Jane",
            "Smith",
            "987654321",
            "jane.smith@example.com",
            "XYZ789");

        // Act
        var response = await _client.GetAsync($"/api/guests/{guestId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var guest = await response.Content.ReadFromJsonAsync<GuestDto>();
        guest.Should().NotBeNull();
        guest!.Id.Should().Be(guestId);
        guest.FirstName.Should().Be("Jane");
        guest.LastName.Should().Be("Smith");
        guest.Phone.Should().Be("987654321");
        guest.Email.Should().Be("jane.smith@example.com");
        guest.DocumentNumber.Should().Be("XYZ789");
    }

    [Fact]
    public async Task GetGuestById_WithNonExistentId_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync($"/api/guests/{Guid.NewGuid()}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}