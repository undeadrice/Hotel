using FluentAssertions;
using Hotel.Application.Guests.TransferObjects;
using Hotel.IntegrationTests.Infrastructure;
using Hotel.IntegrationTests.Infrastructure.TestData;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Hotel.IntegrationTests.Guests.Queries;

public class SearchGuestsQueryTests : IClassFixture<HotelWebApplicationFactory>, IAsyncLifetime
{
    private readonly HotelWebApplicationFactory _factory;

    private HttpClient _client = null!;

    public SearchGuestsQueryTests(HotelWebApplicationFactory factory)
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
    public async Task SearchGuests_WithMatchingName_ReturnsGuest()
    {
        // Arrange
        await GuestTestData.CreateGuestAsync(_client, "Alice", "Johnson", "111222333", "alice.johnson@example.com", "DOC001");
        await GuestTestData.CreateGuestAsync(_client, "Bob", "Brown", "444555666", "bob.brown@example.com", "DOC002");

        // Act
        var response = await _client.GetAsync("/api/guests/search?name=Alice");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var guests = await response.Content.ReadFromJsonAsync<List<GuestListDto>>();
        guests.Should().ContainSingle(g => g.FullName == "Alice Johnson");
    }

    [Fact]
    public async Task SearchGuests_WithMatchingPhone_ReturnsGuest()
    {
        // Arrange
        await GuestTestData.CreateGuestAsync(_client, "Alice", "Johnson", "111222333", "alice.johnson@example.com", "DOC001");
        await GuestTestData.CreateGuestAsync(_client, "Bob", "Brown", "444555666", "bob.brown@example.com", "DOC002");

        // Act
        var response = await _client.GetAsync("/api/guests/search?phone=444555666");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var guests = await response.Content.ReadFromJsonAsync<List<GuestListDto>>();
        guests.Should().ContainSingle(g => g.FullName == "Bob Brown");
    }

    [Fact]
    public async Task SearchGuests_WithMatchingEmail_ReturnsGuest()
    {
        // Arrange
        await GuestTestData.CreateGuestAsync(_client, "Alice", "Johnson", "111222333", "alice.johnson@example.com", "DOC001");
        await GuestTestData.CreateGuestAsync(_client, "Bob", "Brown", "444555666", "bob.brown@example.com", "DOC002");

        // Act
        var response = await _client.GetAsync("/api/guests/search?email=bob.brown%40example.com");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var guests = await response.Content.ReadFromJsonAsync<List<GuestListDto>>();
        guests.Should().ContainSingle(g => g.FullName == "Bob Brown");
    }

    [Fact]
    public async Task SearchGuests_WithMatchingDocumentNumber_ReturnsGuest()
    {
        // Arrange
        await GuestTestData.CreateGuestAsync(_client, "Alice", "Johnson", "111222333", "alice.johnson@example.com", "DOC001");
        await GuestTestData.CreateGuestAsync(_client, "Bob", "Brown", "444555666", "bob.brown@example.com", "DOC002");

        // Act
        var response = await _client.GetAsync("/api/guests/search?documentNumber=DOC002");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var guests = await response.Content.ReadFromJsonAsync<List<GuestListDto>>();
        guests.Should().ContainSingle(g => g.FullName == "Bob Brown");
    }

    [Fact]
    public async Task SearchGuests_WithNoMatchingCriteria_ReturnsEmptyList()
    {
        // Arrange
        await GuestTestData.CreateGuestAsync(_client, "Alice", "Johnson", "111222333", "alice.johnson@example.com", "DOC001");

        // Act
        var response = await _client.GetAsync("/api/guests/search?name=NonExistent");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var guests = await response.Content.ReadFromJsonAsync<List<GuestListDto>>();
        guests.Should().BeEmpty();
    }
}