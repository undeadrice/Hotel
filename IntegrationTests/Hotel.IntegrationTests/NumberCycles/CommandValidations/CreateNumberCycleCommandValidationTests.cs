using FluentAssertions;
using Hotel.Application.NumberCycles.Commands;
using Hotel.Domain.NumberCycles.Enums;
using Hotel.IntegrationTests.Infrastructure;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Hotel.IntegrationTests.NumberCycles.CommandValidations;

public class CreateNumberCycleCommandValidationTests : IClassFixture<HotelWebApplicationFactory>, IAsyncLifetime
{
    private readonly HotelWebApplicationFactory _factory;

    private HttpClient _client = null!;

    public CreateNumberCycleCommandValidationTests(HotelWebApplicationFactory factory)
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
    public async Task CreateNumberCycle_WithInvalidTopic_ReturnsBadRequest()
    {
        // Arrange
        var command = new CreateNumberCycleCommand((NumberCycleTopic)999, "RES", 1);

        // Act
        var response = await _client.PostAsJsonAsync("/api/numbercycles", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateNumberCycle_WithEmptyPrefix_ReturnsBadRequest()
    {
        // Arrange
        var command = new CreateNumberCycleCommand(NumberCycleTopic.Reservation, string.Empty, 1);

        // Act
        var response = await _client.PostAsJsonAsync("/api/numbercycles", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateNumberCycle_WithPrefixExceedingMaxLength_ReturnsBadRequest()
    {
        // Arrange
        var command = new CreateNumberCycleCommand(NumberCycleTopic.Reservation, new string('A', 21), 1);

        // Act
        var response = await _client.PostAsJsonAsync("/api/numbercycles", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateNumberCycle_WithNegativeStartIndex_ReturnsBadRequest()
    {
        // Arrange
        var command = new CreateNumberCycleCommand(NumberCycleTopic.Reservation, "RES", -1);

        // Act
        var response = await _client.PostAsJsonAsync("/api/numbercycles", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}