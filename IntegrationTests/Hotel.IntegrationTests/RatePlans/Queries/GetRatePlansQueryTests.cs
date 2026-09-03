using FluentAssertions;
using Hotel.Application.RatePlans.TransferObjects;
using Hotel.IntegrationTests.Infrastructure;
using Hotel.IntegrationTests.Infrastructure.TestData;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Hotel.IntegrationTests.RatePlans.Queries;

public class GetRatePlansQueryTests : IClassFixture<HotelWebApplicationFactory>, IAsyncLifetime
{
    private readonly HotelWebApplicationFactory _factory;

    private HttpClient _client = null!;

    public GetRatePlansQueryTests(HotelWebApplicationFactory factory)
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
    public async Task GetRatePlans_WhenNoRatePlansExist_ReturnsEmptyList()
    {
        // Act
        var response = await _client.GetAsync("/api/rateplans");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var ratePlans = await response.Content.ReadFromJsonAsync<List<RatePlanListDto>>();
        ratePlans.Should().NotBeNull();
        ratePlans.Should().BeEmpty();
    }

    [Fact]
    public async Task GetRatePlans_AfterCreatingRatePlan_ReturnsRatePlan()
    {
        // Arrange
        var roomTypeId = await RoomTypeTestData.CreateRoomTypeAsync(_client, "Suite");
        var transactionGroupId = await TransactionGroupTestData.CreateTransactionGroupAsync(_client);
        var transactionCodeId = await TransactionCodeTestData.CreateTransactionCodeAsync(_client, transactionGroupId);

        await RatePlanTestData.CreateRatePlanAsync(_client, transactionCodeId, roomTypeId, name: "Peak Season");

        // Act
        var response = await _client.GetAsync("/api/rateplans");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var ratePlans = await response.Content.ReadFromJsonAsync<List<RatePlanListDto>>();
        ratePlans.Should().ContainSingle(rp => rp.Name == "Peak Season");

        var ratePlan = ratePlans!.Single();
        ratePlan.Id.Should().NotBeEmpty();
        ratePlan.Name.Should().Be("Peak Season");
        ratePlan.TransactionCodeId.Should().Be(transactionCodeId);
        ratePlan.StartDate.Should().Be(RatePlanDates.ValidStartDate);
        ratePlan.EndDate.Should().Be(RatePlanDates.ValidEndDate);
        ratePlan.IsActive.Should().BeTrue();
    }
}