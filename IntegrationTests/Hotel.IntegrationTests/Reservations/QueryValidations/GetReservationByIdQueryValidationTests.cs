using FluentAssertions;
using Hotel.IntegrationTests.Infrastructure;
using System.Net;
using Xunit;

namespace Hotel.IntegrationTests.Reservations.QueryValidations;

public class GetReservationByIdQueryValidationTests : IClassFixture<HotelWebApplicationFactory>, IAsyncLifetime
{
    private readonly HotelWebApplicationFactory _factory;

    private HttpClient _client = null!;

    public GetReservationByIdQueryValidationTests(HotelWebApplicationFactory factory)
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
    public async Task GetReservationById_WithEmptyId_ReturnsBadRequest()
    {
        // Act
        var response = await _client.GetAsync($"/api/reservations/{Guid.Empty}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}