using FluentAssertions;
using Hotel.Application.Guests.TransferObjects;
using Hotel.IntegrationTests.Infrastructure;
using Hotel.IntegrationTests.Infrastructure.TestData;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Hotel.IntegrationTests.Guests.Queries;

public class GetGuestsQueryTests : IClassFixture<HotelWebApplicationFactory>, IAsyncLifetime
{
    private readonly HotelWebApplicationFactory _factory;

    private HttpClient _client = null!;

    public GetGuestsQueryTests(HotelWebApplicationFactory factory)
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
    public async Task GetGuests_WhenNoGuestsExist_ReturnsEmptyList()
    {
        // Act
        var response = await _client.GetAsync("/api/guests");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var guests = await response.Content.ReadFromJsonAsync<List<GuestListDto>>();
        guests.Should().NotBeNull();
        guests.Should().BeEmpty();
    }

    [Fact]
    public async Task GetGuests_AfterCreatingGuest_ReturnsGuest()
    {
        // Arrange
        await GuestTestData.CreateGuestAsync(_client, "John", "Doe");

        // Act
        var response = await _client.GetAsync("/api/guests");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var guests = await response.Content.ReadFromJsonAsync<List<GuestListDto>>();
        guests.Should().ContainSingle(g => g.FullName == "John Doe");
    }
}