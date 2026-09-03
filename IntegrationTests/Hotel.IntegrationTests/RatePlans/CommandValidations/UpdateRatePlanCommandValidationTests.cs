using FluentAssertions;
using Hotel.Application.RatePlans.Commands;
using Hotel.IntegrationTests.Infrastructure;
using Hotel.IntegrationTests.Infrastructure.TestData;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Hotel.IntegrationTests.RatePlans.CommandValidations;

public class UpdateRatePlanCommandValidationTests : IClassFixture<HotelWebApplicationFactory>, IAsyncLifetime
{
    private readonly HotelWebApplicationFactory _factory;

    private HttpClient _client = null!;

    public UpdateRatePlanCommandValidationTests(HotelWebApplicationFactory factory)
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
    public async Task UpdateRatePlan_WithEmptyId_ReturnsBadRequest()
    {
        // Arrange
        var command = new UpdateRatePlanCommand(
            Guid.Empty,
            "Updated rate plan",
            Guid.NewGuid(),
            RatePlanDates.ValidStartDate,
            RatePlanDates.ValidEndDate,
            [new UpdateRatePlanRoomCommand(Guid.NewGuid(), 150m)]);

        // Act
        var response = await _client.PutAsJsonAsync("/api/rateplans", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateRatePlan_WithEmptyName_ReturnsBadRequest()
    {
        // Arrange
        var command = new UpdateRatePlanCommand(
            Guid.NewGuid(),
            string.Empty,
            Guid.NewGuid(),
            RatePlanDates.ValidStartDate,
            RatePlanDates.ValidEndDate,
            [new UpdateRatePlanRoomCommand(Guid.NewGuid(), 150m)]);

        // Act
        var response = await _client.PutAsJsonAsync("/api/rateplans", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateRatePlan_WithNameExceedingMaxLength_ReturnsBadRequest()
    {
        // Arrange
        var command = new UpdateRatePlanCommand(
            Guid.NewGuid(),
            new string('A', 101),
            Guid.NewGuid(),
            RatePlanDates.ValidStartDate,
            RatePlanDates.ValidEndDate,
            [new UpdateRatePlanRoomCommand(Guid.NewGuid(), 150m)]);

        // Act
        var response = await _client.PutAsJsonAsync("/api/rateplans", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateRatePlan_WithStartDateAfterEndDate_ReturnsBadRequest()
    {
        // Arrange
        var command = new UpdateRatePlanCommand(
            Guid.NewGuid(),
            "Updated rate plan",
            Guid.NewGuid(),
            RatePlanDates.ValidEndDate,
            RatePlanDates.ValidStartDate,
            [new UpdateRatePlanRoomCommand(Guid.NewGuid(), 150m)]);

        // Act
        var response = await _client.PutAsJsonAsync("/api/rateplans", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateRatePlan_WithEmptyTransactionCodeId_ReturnsBadRequest()
    {
        // Arrange
        var command = new UpdateRatePlanCommand(
            Guid.NewGuid(),
            "Updated rate plan",
            Guid.Empty,
            RatePlanDates.ValidStartDate,
            RatePlanDates.ValidEndDate,
            [new UpdateRatePlanRoomCommand(Guid.NewGuid(), 150m)]);

        // Act
        var response = await _client.PutAsJsonAsync("/api/rateplans", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateRatePlan_WithNegativeRoomPrice_ReturnsBadRequest()
    {
        // Arrange
        var command = new UpdateRatePlanCommand(
            Guid.NewGuid(),
            "Updated rate plan",
            Guid.NewGuid(),
            RatePlanDates.ValidStartDate,
            RatePlanDates.ValidEndDate,
            [new UpdateRatePlanRoomCommand(Guid.NewGuid(), -1m)]);

        // Act
        var response = await _client.PutAsJsonAsync("/api/rateplans", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateRatePlan_WithEmptyRoomTypeId_ReturnsBadRequest()
    {
        // Arrange
        var command = new UpdateRatePlanCommand(
            Guid.NewGuid(),
            "Updated rate plan",
            Guid.NewGuid(),
            RatePlanDates.ValidStartDate,
            RatePlanDates.ValidEndDate,
            [new UpdateRatePlanRoomCommand(Guid.Empty, 150m)]);

        // Act
        var response = await _client.PutAsJsonAsync("/api/rateplans", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateRatePlan_WithEmptyRoomsList_ReturnsBadRequest()
    {
        // Arrange
        var command = new UpdateRatePlanCommand(
            Guid.NewGuid(),
            "Updated rate plan",
            Guid.NewGuid(),
            RatePlanDates.ValidStartDate,
            RatePlanDates.ValidEndDate,
            []);

        // Act
        var response = await _client.PutAsJsonAsync("/api/rateplans", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}