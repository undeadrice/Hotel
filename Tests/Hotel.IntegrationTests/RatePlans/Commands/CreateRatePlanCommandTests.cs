using FluentAssertions;
using Hotel.Application.RatePlans.Commands;
using Hotel.IntegrationTests.Infrastructure;
using Hotel.IntegrationTests.Infrastructure.TestData;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Hotel.IntegrationTests.RatePlans.Commands;

public class CreateRatePlanCommandTests : IClassFixture<HotelWebApplicationFactory>, IAsyncLifetime
{
    private readonly HotelWebApplicationFactory _factory;

    private HttpClient _client = null!;

    public CreateRatePlanCommandTests(HotelWebApplicationFactory factory)
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
    public async Task CreateRatePlan_WithValidCommand_ReturnsRatePlanId()
    {
        // Arrange
        var roomTypeId = await RoomTypeTestData.CreateRoomTypeAsync(_client);
        var transactionGroupId = await TransactionGroupTestData.CreateTransactionGroupAsync(_client);
        var transactionCodeId = await TransactionCodeTestData.CreateTransactionCodeAsync(_client, transactionGroupId);

        // Act
        var ratePlanId = await RatePlanTestData.CreateRatePlanAsync(_client, transactionCodeId, roomTypeId);

        // Assert
        ratePlanId.Should().NotBeEmpty();

        var getResponse = await _client.GetAsync($"/api/rateplans/{ratePlanId}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task CreateRatePlan_WithStartDateBeforeBusinessDate_ReturnsBadRequest()
    {
        // Arrange
        var roomTypeId = await RoomTypeTestData.CreateRoomTypeAsync(_client);
        var transactionGroupId = await TransactionGroupTestData.CreateTransactionGroupAsync(_client);
        var transactionCodeId = await TransactionCodeTestData.CreateTransactionCodeAsync(_client, transactionGroupId);

        var command = new CreateRatePlanCommand(
            "Past rate plan",
            transactionCodeId,
            RatePlanDates.BusinessDate.AddDays(-1),
            RatePlanDates.ValidEndDate,
            [new CreateRatePlanRoomCommand(roomTypeId, 100m)]);

        // Act
        var response = await _client.PostAsJsonAsync("/api/rateplans", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}