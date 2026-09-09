using FluentAssertions;
using Hotel.Application.Transactions.Commands;
using Hotel.Application.Transactions.TransferObjects;
using Hotel.Domain.Transactions.Enums;
using Hotel.IntegrationTests.Infrastructure;
using Hotel.IntegrationTests.Infrastructure.TestData;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Hotel.IntegrationTests.Transactions.Commands;

public class UpdateTransactionCodeCommandTests : IClassFixture<HotelWebApplicationFactory>, IAsyncLifetime
{
    private readonly HotelWebApplicationFactory _factory;

    private HttpClient _client = null!;

    public UpdateTransactionCodeCommandTests(HotelWebApplicationFactory factory)
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
    public async Task UpdateTransactionCode_WithValidCommand_ReturnsNoContent()
    {
        // Arrange
        var chargeGroupId = await TransactionGroupTestData.CreateTransactionGroupAsync(_client, "CHARGE", "Charge Group");
        var paymentGroupId = await TransactionGroupTestData.CreateTransactionGroupAsync(_client, "PAYMENT", "Payment Group", TransactionType.Payment);
        var transactionCodeId = await TransactionCodeTestData.CreateTransactionCodeAsync(_client, chargeGroupId, "ROOM", "Room Charge");
        var command = new UpdateTransactionCodeCommand(transactionCodeId, paymentGroupId, "ROOM2", "Room Charge Updated");

        // Act
        var response = await _client.PutAsJsonAsync("/api/transactioncodes", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var getResponse = await _client.GetAsync($"/api/transactioncodes/{transactionCodeId}");
        var transactionCode = await getResponse.Content.ReadFromJsonAsync<TransactionCodeDto>();
        transactionCode.Should().NotBeNull();
        transactionCode!.TransactionGroupId.Should().Be(paymentGroupId);
        transactionCode.TransactionGroupName.Should().Be("Payment Group");
        transactionCode.Code.Should().Be("ROOM2");
        transactionCode.Name.Should().Be("Room Charge Updated");
    }

    [Fact]
    public async Task UpdateTransactionCode_WithNonExistentId_ReturnsNotFound()
    {
        // Arrange
        var transactionGroupId = await TransactionGroupTestData.CreateTransactionGroupAsync(_client);
        var command = new UpdateTransactionCodeCommand(Guid.NewGuid(), transactionGroupId, "ROOM", "Room Charge");

        // Act
        var response = await _client.PutAsJsonAsync("/api/transactioncodes", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateTransactionCode_WithDuplicateCode_ReturnsBadRequest()
    {
        // Arrange
        var transactionGroupId = await TransactionGroupTestData.CreateTransactionGroupAsync(_client, "CHARGE", "Charge Group");
        var transactionCodeId = await TransactionCodeTestData.CreateTransactionCodeAsync(_client, transactionGroupId, "ROOM", "Room Charge");
        await TransactionCodeTestData.CreateTransactionCodeAsync(_client, transactionGroupId, "TAX", "Tax");
        
        var command = new UpdateTransactionCodeCommand(transactionCodeId, transactionGroupId, "TAX", "Room Charge Updated");

        // Act
        var response = await _client.PutAsJsonAsync("/api/transactioncodes", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}