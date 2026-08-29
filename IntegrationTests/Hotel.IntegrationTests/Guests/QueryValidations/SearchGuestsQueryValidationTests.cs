using FluentAssertions;
using Hotel.IntegrationTests.Infrastructure;
using System.Net;
using Xunit;

namespace Hotel.IntegrationTests.Guests.QueryValidations;

public class SearchGuestsQueryValidationTests : IClassFixture<HotelWebApplicationFactory>, IAsyncLifetime
{
    private readonly HotelWebApplicationFactory _factory;

    private HttpClient _client = null!;

    public SearchGuestsQueryValidationTests(HotelWebApplicationFactory factory)
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
    public async Task SearchGuests_WithNoCriteria_ReturnsBadRequest()
    {
        // Act
        var response = await _client.GetAsync("/api/guests/search");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}