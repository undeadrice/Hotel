using FluentAssertions;
using Hotel.Application.Transactions.Commands;
using Hotel.Application.Transactions.TransferObjects;
using Hotel.IntegrationTests.Infrastructure;
using Hotel.IntegrationTests.Infrastructure.TestData;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Hotel.IntegrationTests.Transactions.Queries;

public class GetTransactionCodesQueryTests : IClassFixture<HotelWebApplicationFactory>, IAsyncLifetime
{
    private readonly HotelWebApplicationFactory _factory;

    private HttpClient _client = null!;

    public GetTransactionCodesQueryTests(HotelWebApplicationFactory factory)
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
    public async Task GetTransactionCodes_WhenCodesExist_ReturnsCodes()
    {
        // Arrange
        var transactionGroupId = await TransactionGroupTestData.CreateTransactionGroupAsync(_client);
        await TransactionCodeTestData.CreateTransactionCodeAsync(_client, transactionGroupId, "ROOM", "Room Charge");
        await TransactionCodeTestData.CreateTransactionCodeAsync(_client, transactionGroupId, "TAX", "Tax");

        // Act
        var response = await _client.GetAsync("/api/transactioncodes");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var transactionCodes = await response.Content.ReadFromJsonAsync<List<TransactionCodeListDto>>();
        transactionCodes.Should().NotBeNull();
        transactionCodes!.Should().HaveCount(2);

        transactionCodes.Should().ContainSingle(c =>
            c.Id != Guid.Empty &&
            c.TransactionGroupId == transactionGroupId &&
            c.TransactionGroupName == "Charges" &&
            c.Code == "ROOM" &&
            c.Name == "Room Charge" &&
            c.IsActive);

        transactionCodes.Should().ContainSingle(c =>
            c.Id != Guid.Empty &&
            c.TransactionGroupId == transactionGroupId &&
            c.TransactionGroupName == "Charges" &&
            c.Code == "TAX" &&
            c.Name == "Tax" &&
            c.IsActive);
    }

    [Fact]
    public async Task GetTransactionCodes_WithActiveFilter_ReturnsOnlyActiveCodes()
    {
        // Arrange
        var transactionGroupId = await TransactionGroupTestData.CreateTransactionGroupAsync(_client);
        var activeCodeId = await TransactionCodeTestData.CreateTransactionCodeAsync(_client, transactionGroupId, "ROOM", "Room Charge");
        var inactiveCodeId = await TransactionCodeTestData.CreateTransactionCodeAsync(_client, transactionGroupId, "TAX", "Tax");

        await _client.PutAsJsonAsync(
            "/api/transactioncodes/status",
            new ChangeTransactionCodeStatusCommand(inactiveCodeId, false));

        // Act
        var response = await _client.GetAsync("/api/transactioncodes?isActive=true");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var transactionCodes = await response.Content.ReadFromJsonAsync<List<TransactionCodeListDto>>();
        transactionCodes.Should().NotBeNull();
        transactionCodes!.Should().ContainSingle();

        transactionCodes[0].Should().BeEquivalentTo(new
        {
            Id = activeCodeId,
            TransactionGroupId = transactionGroupId,
            TransactionGroupName = "Charges",
            Code = "ROOM",
            Name = "Room Charge",
            IsActive = true
        });
    }

    [Fact]
    public async Task GetTransactionCodes_WithTransactionGroupFilter_ReturnsOnlyGroupCodes()
    {
        // Arrange
        var groupAId = await TransactionGroupTestData.CreateTransactionGroupAsync(_client, "CHARGE", "Charge Group");
        var groupBId = await TransactionGroupTestData.CreateTransactionGroupAsync(_client, "PAYMENT", "Payment Group");

        await TransactionCodeTestData.CreateTransactionCodeAsync(_client, groupAId, "ROOM", "Room Charge");
        await TransactionCodeTestData.CreateTransactionCodeAsync(_client, groupBId, "TAX", "Tax");

        // Act
        var response = await _client.GetAsync($"/api/transactioncodes?transactionGroupId={groupAId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var transactionCodes = await response.Content.ReadFromJsonAsync<List<TransactionCodeListDto>>();
        transactionCodes.Should().NotBeNull();
        transactionCodes!.Should().HaveCount(1);
        transactionCodes[0].Should().BeEquivalentTo(new
        {
            TransactionGroupId = groupAId,
            TransactionGroupName = "Charge Group",
            Code = "ROOM",
            Name = "Room Charge",
            IsActive = true
        });
    }
}