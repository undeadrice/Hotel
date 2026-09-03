using FluentAssertions;
using Hotel.Application.RatePlans.Commands;
using Hotel.IntegrationTests.Infrastructure;
using Hotel.IntegrationTests.Infrastructure.TestData;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Hotel.IntegrationTests.RatePlans.CommandValidations;

public class CreateRatePlanCommandValidationTests : IClassFixture<HotelWebApplicationFactory>, IAsyncLifetime
{
    private readonly HotelWebApplicationFactory _factory;

    private HttpClient _client = null!;

    public CreateRatePlanCommandValidationTests(HotelWebApplicationFactory factory)
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
    public async Task CreateRatePlan_WithEmptyName_ReturnsBadRequest()
    {
        // Arrange
        var command = new CreateRatePlanCommand(
            string.Empty,
            Guid.NewGuid(),
            RatePlanDates.ValidStartDate,
            RatePlanDates.ValidEndDate,
            [new CreateRatePlanRoomCommand(Guid.NewGuid(), 100m)]);

        // Act
        var response = await _client.PostAsJsonAsync("/api/rateplans", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateRatePlan_WithEmptyTransactionCodeId_ReturnsBadRequest()
    {
        // Arrange
        var command = new CreateRatePlanCommand(
            "Valid name",
            Guid.Empty,
            RatePlanDates.ValidStartDate,
            RatePlanDates.ValidEndDate,
            [new CreateRatePlanRoomCommand(Guid.NewGuid(), 100m)]);

        // Act
        var response = await _client.PostAsJsonAsync("/api/rateplans", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateRatePlan_WithStartDateAfterEndDate_ReturnsBadRequest()
    {
        // Arrange
        var command = new CreateRatePlanCommand(
            "Valid name",
            Guid.NewGuid(),
            RatePlanDates.ValidEndDate,
            RatePlanDates.ValidStartDate,
            [new CreateRatePlanRoomCommand(Guid.NewGuid(), 100m)]);

        // Act
        var response = await _client.PostAsJsonAsync("/api/rateplans", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateRatePlan_WithEmptyRoomsList_ReturnsBadRequest()
    {
        // Arrange
        var command = new CreateRatePlanCommand(
            "Valid name",
            Guid.NewGuid(),
            RatePlanDates.ValidStartDate,
            RatePlanDates.ValidEndDate,
            []);

        // Act
        var response = await _client.PostAsJsonAsync("/api/rateplans", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateRatePlan_WithNegativeRoomPrice_ReturnsBadRequest()
    {
        // Arrange
        var command = new CreateRatePlanCommand(
            "Valid name",
            Guid.NewGuid(),
            RatePlanDates.ValidStartDate,
            RatePlanDates.ValidEndDate,
            [new CreateRatePlanRoomCommand(Guid.NewGuid(), -1m)]);

        // Act
        var response = await _client.PostAsJsonAsync("/api/rateplans", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}