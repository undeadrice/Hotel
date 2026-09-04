using FluentAssertions;
using Hotel.Application.RatePlans.TransferObjects;
using Hotel.IntegrationTests.Infrastructure;
using Hotel.IntegrationTests.Infrastructure.TestData;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Hotel.IntegrationTests.RatePlans.Queries;

public class GetRatePlansByRoomTypeQueryTests : IClassFixture<HotelWebApplicationFactory>, IAsyncLifetime
{
    private readonly HotelWebApplicationFactory _factory;

    private HttpClient _client = null!;

    public GetRatePlansByRoomTypeQueryTests(HotelWebApplicationFactory factory)
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
    public async Task GetRatePlansByRoom_WithMatchingRatePlan_ReturnsRatePlans()
    {
        // Arrange
        var roomTypeId = await RoomTypeTestData.CreateRoomTypeAsync(_client, "King");
        var roomId = await RoomTestData.CreateRoomAsync(_client, "301", roomTypeId);

        var transactionGroupId = await TransactionGroupTestData.CreateTransactionGroupAsync(_client);
        var transactionCodeId = await TransactionCodeTestData.CreateTransactionCodeAsync(_client, transactionGroupId);

        await RatePlanTestData.CreateRatePlanAsync(_client, transactionCodeId, roomTypeId, name: "Royal Rate");

        var startDate = RatePlanDates.ValidStartDate;
        var endDate = RatePlanDates.ValidEndDate;

        // Act
        var response = await _client.GetAsync(
            $"/api/rateplans/by-room/{roomId}?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var ratePlans = await response.Content.ReadFromJsonAsync<List<RatePlanListSimpleDto>>();
        ratePlans.Should().ContainSingle(rp => rp.Name == "Royal Rate");
    }

    [Fact]
    public async Task GetRatePlansByRoom_WithMultipleMatchingRatePlans_ReturnsAllRatePlans()
    {
        // Arrange
        var roomTypeId = await RoomTypeTestData.CreateRoomTypeAsync(_client, "Queen");
        var roomId = await RoomTestData.CreateRoomAsync(_client, "302", roomTypeId);

        var transactionGroupId = await TransactionGroupTestData.CreateTransactionGroupAsync(_client);
        var transactionCodeId = await TransactionCodeTestData.CreateTransactionCodeAsync(_client, transactionGroupId);

        var startDate = RatePlanDates.ValidStartDate;
        var endDate = RatePlanDates.ValidEndDate;

        await RatePlanTestData.CreateRatePlanAsync(_client, transactionCodeId, roomTypeId, name: "Royal Rate");
        await RatePlanTestData.CreateRatePlanAsync(
            _client,
            transactionCodeId,
            roomTypeId,
            name: "Standard Rate",
            startDate: startDate.AddDays(-1),
            endDate: endDate.AddDays(1));
        await RatePlanTestData.CreateRatePlanAsync(
            _client,
            transactionCodeId,
            roomTypeId,
            name: "Future Rate",
            startDate: startDate.AddDays(30),
            endDate: startDate.AddDays(60));

        // Act
        var response = await _client.GetAsync(
            $"/api/rateplans/by-room/{roomId}?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var ratePlans = await response.Content.ReadFromJsonAsync<List<RatePlanListSimpleDto>>();
        ratePlans.Should().HaveCount(2);
        ratePlans.Should().Contain(rp => rp.Name == "Royal Rate");
        ratePlans.Should().Contain(rp => rp.Name == "Standard Rate");
        ratePlans.Should().NotContain(rp => rp.Name == "Future Rate");
    }

    [Fact]
    public async Task GetRatePlansByRoom_WhenNoMatchingRatePlan_ReturnsEmptyList()
    {
        // Arrange
        var roomTypeId = await RoomTypeTestData.CreateRoomTypeAsync(_client, "Twin");
        var roomId = await RoomTestData.CreateRoomAsync(_client, "401", roomTypeId);

        var startDate = RatePlanDates.ValidStartDate;
        var endDate = RatePlanDates.ValidEndDate;

        // Act
        var response = await _client.GetAsync(
            $"/api/rateplans/by-room/{roomId}?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var ratePlans = await response.Content.ReadFromJsonAsync<List<RatePlanListSimpleDto>>();
        ratePlans.Should().NotBeNull();
        ratePlans.Should().BeEmpty();
    }
}