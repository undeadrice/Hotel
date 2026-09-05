using FluentAssertions;
using Hotel.IntegrationTests.Infrastructure;
using System.Net;
using Xunit;

namespace Hotel.IntegrationTests.NumberCycles.CommandValidations;

public class DeleteNumberCycleCommandValidationTests : IClassFixture<HotelWebApplicationFactory>, IAsyncLifetime
{
    private readonly HotelWebApplicationFactory _factory;

    private HttpClient _client = null!;

    public DeleteNumberCycleCommandValidationTests(HotelWebApplicationFactory factory)
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
    public async Task DeleteNumberCycle_WithEmptyId_ReturnsBadRequest()
    {
        // Act
        var response = await _client.DeleteAsync($"/api/numbercycles/{Guid.Empty}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}