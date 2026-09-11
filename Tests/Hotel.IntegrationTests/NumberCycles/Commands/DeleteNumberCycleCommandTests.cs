using FluentAssertions;
using Hotel.Domain.NumberCycles.Enums;
using Hotel.IntegrationTests.Infrastructure;
using Hotel.IntegrationTests.Infrastructure.TestData;
using System.Net;
using Xunit;

namespace Hotel.IntegrationTests.NumberCycles.Commands;

public class DeleteNumberCycleCommandTests : IClassFixture<HotelWebApplicationFactory>, IAsyncLifetime
{
    private readonly HotelWebApplicationFactory _factory;

    private HttpClient _client = null!;

    public DeleteNumberCycleCommandTests(HotelWebApplicationFactory factory)
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
    public async Task DeleteNumberCycle_WithExistingCycle_ReturnsNoContent()
    {
        // Arrange
        var numberCycleId = await NumberCycleTestData.CreateNumberCycleAsync(_client, NumberCycleTopic.FiscalAccount, "FA", 1);

        // Act
        var response = await _client.DeleteAsync($"/api/numbercycles/{numberCycleId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var getResponse = await _client.GetAsync($"/api/numbercycles/{numberCycleId}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteNumberCycle_WithNonExistentId_ReturnsNotFound()
    {
        // Act
        var response = await _client.DeleteAsync($"/api/numbercycles/{Guid.NewGuid()}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}