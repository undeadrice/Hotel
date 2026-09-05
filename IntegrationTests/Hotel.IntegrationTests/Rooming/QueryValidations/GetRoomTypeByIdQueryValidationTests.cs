using FluentAssertions;
using Hotel.IntegrationTests.Infrastructure;
using System.Net;
using Xunit;

namespace Hotel.IntegrationTests.Rooming.QueryValidations;

public class GetRoomTypeByIdQueryValidationTests : IClassFixture<HotelWebApplicationFactory>, IAsyncLifetime
{
    private readonly HotelWebApplicationFactory _factory;

    private HttpClient _client = null!;

    public GetRoomTypeByIdQueryValidationTests(HotelWebApplicationFactory factory)
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
    public async Task GetRoomTypeById_WithEmptyId_ReturnsBadRequest()
    {
        // Arrange
        var emptyId = Guid.Empty;

        // Act
        var response = await _client.GetAsync($"/api/roomtypes/{emptyId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}