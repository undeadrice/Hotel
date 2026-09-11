using FluentAssertions;
using Hotel.Application.NumberCycles.Commands;
using Hotel.Domain.NumberCycles.Enums;
using Hotel.IntegrationTests.Infrastructure;
using Hotel.IntegrationTests.Infrastructure.TestData;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Hotel.IntegrationTests.NumberCycles.Commands;

public class CreateNumberCycleCommandTests : IClassFixture<HotelWebApplicationFactory>, IAsyncLifetime
{
    private readonly HotelWebApplicationFactory _factory;

    private HttpClient _client = null!;

    public CreateNumberCycleCommandTests(HotelWebApplicationFactory factory)
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
    public async Task CreateNumberCycle_WithValidCommand_ReturnsCycleId()
    {
        // Arrange
        var command = new CreateNumberCycleCommand(NumberCycleTopic.Reservation, "RES", 1);

        // Act
        var response = await _client.PostAsJsonAsync("/api/numbercycles", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var numberCycleId = await response.Content.ReadFromJsonAsync<Guid>();
        numberCycleId.Should().NotBeEmpty();
    }

    [Fact]
    public async Task CreateNumberCycle_WithDuplicateTopic_ReturnsBadRequest()
    {
        // Arrange
        await NumberCycleTestData.CreateNumberCycleAsync(_client, NumberCycleTopic.Reservation, "RES", 1);

        var command = new CreateNumberCycleCommand(NumberCycleTopic.Reservation, "OTHER", 5);

        // Act
        var response = await _client.PostAsJsonAsync("/api/numbercycles", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}