using FluentAssertions;
using Hotel.Application.Transactions.Commands;
using Hotel.Application.Transactions.TransferObjects;
using Hotel.IntegrationTests.Infrastructure;
using Hotel.IntegrationTests.Infrastructure.TestData;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Hotel.IntegrationTests.Transactions.Queries;

public class GetTransactionCodesSimpleListQueryTests : IClassFixture<HotelWebApplicationFactory>, IAsyncLifetime
{
    private readonly HotelWebApplicationFactory _factory;

    private HttpClient _client = null!;

    public GetTransactionCodesSimpleListQueryTests(HotelWebApplicationFactory factory)
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
    public async Task GetTransactionCodesSimpleList_WhenActiveCodesExist_ReturnsOnlyActiveCodes()
    {
        // Arrange
        var transactionGroupId = await TransactionGroupTestData.CreateTransactionGroupAsync(_client);

        var activeCodeId = await TransactionCodeTestData.CreateTransactionCodeAsync(_client, transactionGroupId, "ROOM", "Room Charge");
        var inactiveCodeId = await TransactionCodeTestData.CreateTransactionCodeAsync(_client, transactionGroupId, "TAX", "Tax");

        await _client.PutAsJsonAsync(
            "/api/transactioncodes/status",
            new ChangeTransactionCodeStatusCommand(inactiveCodeId, false));

        // Act
        var response = await _client.GetAsync("/api/transactioncodes/simple-list");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var transactionCodes = await response.Content.ReadFromJsonAsync<List<TransactionCodeSimpleListDto>>();
        transactionCodes.Should().NotBeNull();
        transactionCodes!.Should().HaveCount(1);
        transactionCodes[0].Id.Should().Be(activeCodeId);
    }
}