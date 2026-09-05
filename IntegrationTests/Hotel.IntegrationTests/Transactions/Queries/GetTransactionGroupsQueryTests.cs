using FluentAssertions;
using Hotel.Application.Transactions.TransferObjects;
using Hotel.Domain.Transactions.Enums;
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
        var chargeGroupId = await TransactionGroupTestData.CreateTransactionGroupAsync(_client, "CHARGE", "Charge Group", TransactionType.Charge);
        var paymentGroupId = await TransactionGroupTestData.CreateTransactionGroupAsync(_client, "PAYMENT", "Payment Group", TransactionType.Payment);

        // Act
        var response = await _client.GetAsync("/api/transactiongroups");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var transactionGroups = await response.Content.ReadFromJsonAsync<List<TransactionGroupListDto>>();
        transactionGroups.Should().NotBeNull();
        transactionGroups!.Should().HaveCount(2);

        transactionGroups.Should().ContainSingle(g =>
            g.Id == chargeGroupId &&
            g.Code == "CHARGE" &&
            g.Name == "Charge Group" &&
            g.Type == TransactionType.Charge &&
            g.IsActive &&
            g.TransactionCodesCount == 0);

        transactionGroups.Should().ContainSingle(g =>
            g.Id == paymentGroupId &&
            g.Code == "PAYMENT" &&
            g.Name == "Payment Group" &&
            g.Type == TransactionType.Payment &&
            g.IsActive &&
            g.TransactionCodesCount == 0);
    }
}