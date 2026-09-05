using FluentAssertions;
using Hotel.Application.RatePlans.Commands;
using Hotel.IntegrationTests.Infrastructure;
using Hotel.IntegrationTests.Infrastructure.TestData;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Hotel.IntegrationTests.RatePlans.Commands;

public class UpdateRatePlanCommandTests : IClassFixture<HotelWebApplicationFactory>, IAsyncLifetime
{
    private readonly HotelWebApplicationFactory _factory;

    private HttpClient _client = null!;

    public UpdateRatePlanCommandTests(HotelWebApplicationFactory factory)
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
    public async Task UpdateRatePlan_WithValidCommand_ReturnsNoContent()
    {
        // Arrange
        var roomTypeId = await RoomTypeTestData.CreateRoomTypeAsync(_client, "StandardChanged");
        var transactionGroupId = await TransactionGroupTestData.CreateTransactionGroupAsync(_client);
        var transactionCodeId = await TransactionCodeTestData.CreateTransactionCodeAsync(_client, transactionGroupId);

        var ratePlanId = await RatePlanTestData.CreateRatePlanAsync(_client, transactionCodeId, roomTypeId);

        var command = new UpdateRatePlanCommand(
            ratePlanId,
            "Updated rate plan",
            transactionCodeId,
            RatePlanDates.ValidStartDate.AddDays(1),
            RatePlanDates.ValidEndDate,
            [new UpdateRatePlanRoomCommand(roomTypeId, 150m)]);

        // Act
        var response = await _client.PutAsJsonAsync("/api/rateplans", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task UpdateRatePlan_WithNonExistentId_ReturnsNotFound()
    {
        // Arrange
        var roomTypeId = await RoomTypeTestData.CreateRoomTypeAsync(_client);
        var transactionGroupId = await TransactionGroupTestData.CreateTransactionGroupAsync(_client);
        var transactionCodeId = await TransactionCodeTestData.CreateTransactionCodeAsync(_client, transactionGroupId);

        var command = new UpdateRatePlanCommand(
            Guid.NewGuid(),
            "Updated rate plan",
            transactionCodeId,
            RatePlanDates.ValidStartDate,
            RatePlanDates.ValidEndDate,
            [new UpdateRatePlanRoomCommand(roomTypeId, 150m)]);

        // Act
        var response = await _client.PutAsJsonAsync("/api/rateplans", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateRatePlan_WithStartDateBeforeBusinessDate_ReturnsBadRequest()
    {
        // Arrange
        var roomTypeId = await RoomTypeTestData.CreateRoomTypeAsync(_client);
        var transactionGroupId = await TransactionGroupTestData.CreateTransactionGroupAsync(_client);
        var transactionCodeId = await TransactionCodeTestData.CreateTransactionCodeAsync(_client, transactionGroupId);

        var ratePlanId = await RatePlanTestData.CreateRatePlanAsync(_client, transactionCodeId, roomTypeId);

        var command = new UpdateRatePlanCommand(
            ratePlanId,
            "Updated rate plan",
            transactionCodeId,
            RatePlanDates.BusinessDate.AddDays(-1),
            RatePlanDates.ValidEndDate,
            [new UpdateRatePlanRoomCommand(roomTypeId, 150m)]);

        // Act
        var response = await _client.PutAsJsonAsync("/api/rateplans", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}