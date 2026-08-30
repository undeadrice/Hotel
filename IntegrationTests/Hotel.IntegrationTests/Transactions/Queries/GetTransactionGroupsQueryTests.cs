using FluentAssertions;
using Hotel.Application.Transactions.TransferObjects;
using Hotel.IntegrationTests.Infrastructure;
using Hotel.IntegrationTests.Infrastructure.TestData;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Hotel.IntegrationTests.Transactions.Queries;

public class GetTransactionGroupsQueryTests : IClassFixture<HotelWebApplicationFactory>, IAsyncLifetime
{
    private readonly HotelWebApplicationFactory _factory;

    private HttpClient _client = null!;

    public GetTransactionGroupsQueryTests(HotelWebApplicationFactory factory)
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
    public async Task GetTransactionGroups_WhenGroupsExist_ReturnsGroups()
    {
        // Arrange
        await TransactionGroupTestData.CreateTransactionGroupAsync(_client, "CHARGE", "Charge Group");
        await TransactionGroupTestData.CreateTransactionGroupAsync(_client, "PAYMENT", "Payment Group");

        // Act
        var response = await _client.GetAsync("/api/transactiongroups");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var transactionGroups = await response.Content.ReadFromJsonAsync<List<TransactionGroupListDto>>();
        transactionGroups.Should().NotBeNull();
        transactionGroups!.Should().HaveCount(2);
    }
}