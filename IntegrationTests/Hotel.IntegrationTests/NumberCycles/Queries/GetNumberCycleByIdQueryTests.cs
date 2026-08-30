using FluentAssertions;
using Hotel.Application.NumberCycles.TransferObjects;
using Hotel.Domain.NumberCycles.Enums;
using Hotel.IntegrationTests.Infrastructure;
using Hotel.IntegrationTests.Infrastructure.TestData;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Hotel.IntegrationTests.NumberCycles.Queries;

public class GetNumberCycleByIdQueryTests : IClassFixture<HotelWebApplicationFactory>, IAsyncLifetime
{
    private readonly HotelWebApplicationFactory _factory;

    private HttpClient _client = null!;

    public GetNumberCycleByIdQueryTests(HotelWebApplicationFactory factory)
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
    public async Task GetNumberCycleById_WithExistingCycle_ReturnsCycle()
    {
        // Arrange
        var numberCycleId = await NumberCycleTestData.CreateNumberCycleAsync(_client, NumberCycleTopic.FiscalAccount, "FA", 10);

        // Act
        var response = await _client.GetAsync($"/api/numbercycles/{numberCycleId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var numberCycle = await response.Content.ReadFromJsonAsync<NumberCycleDto>();
        numberCycle.Should().NotBeNull();
        numberCycle!.Id.Should().Be(numberCycleId);
        numberCycle.Topic.Should().Be(NumberCycleTopic.FiscalAccount);
        numberCycle.Prefix.Should().Be("FA");
        numberCycle.StartIndex.Should().Be(10);
        numberCycle.CurrentIndex.Should().Be(10);
    }

    [Fact]
    public async Task GetNumberCycleById_WithNonExistentId_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync($"/api/numbercycles/{Guid.NewGuid()}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}