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

public class UpdateTransactionGroupCommandTests : IClassFixture<HotelWebApplicationFactory>, IAsyncLifetime
{
    private readonly HotelWebApplicationFactory _factory;

    private HttpClient _client = null!;

    public UpdateTransactionGroupCommandTests(HotelWebApplicationFactory factory)
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
    public async Task UpdateTransactionGroup_WithValidCommand_ReturnsNoContent()
    {
        // Arrange
        var transactionGroupId = await TransactionGroupTestData.CreateTransactionGroupAsync(_client, "CHARGE", "Charge Group");
        var command = new UpdateTransactionGroupCommand(transactionGroupId, "PAYMENT", "Payment Group", TransactionType.Payment);

        // Act
        var response = await _client.PutAsJsonAsync("/api/transactiongroups", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var getResponse = await _client.GetAsync($"/api/transactiongroups/{transactionGroupId}");
        var transactionGroup = await getResponse.Content.ReadFromJsonAsync<TransactionGroupDto>();
        transactionGroup.Should().NotBeNull();
        transactionGroup!.Code.Should().Be("PAYMENT");
        transactionGroup.Name.Should().Be("Payment Group");
        transactionGroup.Type.Should().Be(TransactionType.Payment);
    }

    [Fact]
    public async Task UpdateTransactionGroup_WithNonExistentId_ReturnsNotFound()
    {
        // Arrange
        var command = new UpdateTransactionGroupCommand(Guid.NewGuid(), "CHARGE", "Charge Group", TransactionType.Charge);

        // Act
        var response = await _client.PutAsJsonAsync("/api/transactiongroups", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}