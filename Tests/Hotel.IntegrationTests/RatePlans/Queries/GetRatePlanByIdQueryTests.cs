using FluentAssertions;
using Hotel.Application.RatePlans.TransferObjects;
using Hotel.IntegrationTests.Infrastructure;
using Hotel.IntegrationTests.Infrastructure.TestData;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Hotel.IntegrationTests.RatePlans.Queries;

public class GetRatePlanByIdQueryTests : IClassFixture<HotelWebApplicationFactory>, IAsyncLifetime
{
    private readonly HotelWebApplicationFactory _factory;

    private HttpClient _client = null!;

    public GetRatePlanByIdQueryTests(HotelWebApplicationFactory factory)
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
    public async Task GetRatePlanById_WithValidId_ReturnsRatePlan()
    {
        // Arrange
        var roomTypeId = await RoomTypeTestData.CreateRoomTypeAsync(_client, "Deluxe");
        var transactionGroupId = await TransactionGroupTestData.CreateTransactionGroupAsync(_client);
        var transactionCodeId = await TransactionCodeTestData.CreateTransactionCodeAsync(_client, transactionGroupId, name: "Stay");

        var ratePlanId = await RatePlanTestData.CreateRatePlanAsync(
            _client,
            transactionCodeId,
            roomTypeId,
            name: "Summer Special",
            price: 250m);

        // Act
        var response = await _client.GetAsync($"/api/rateplans/{ratePlanId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var ratePlan = await response.Content.ReadFromJsonAsync<RatePlanDto>();
        ratePlan.Should().NotBeNull();
        ratePlan!.Name.Should().Be("Summer Special");
        ratePlan.TransactionCode.Should().Be("Stay");
        ratePlan.Rooms.Should().ContainSingle();
        ratePlan.Rooms.Single().RoomType.Should().Be("Deluxe");
        ratePlan.Rooms.Single().Price.Should().Be(250m);
    }

    [Fact]
    public async Task GetRatePlanById_WithNonExistentId_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync($"/api/rateplans/{Guid.NewGuid()}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}