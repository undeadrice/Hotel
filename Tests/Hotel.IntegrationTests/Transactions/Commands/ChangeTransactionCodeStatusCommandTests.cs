using FluentAssertions;
using Hotel.Application.Transactions.Commands;
using Hotel.IntegrationTests.Infrastructure;
using Hotel.IntegrationTests.Infrastructure.TestData;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Hotel.IntegrationTests.Transactions.Commands;

public class ChangeTransactionCodeStatusCommandTests : IClassFixture<HotelWebApplicationFactory>, IAsyncLifetime
{
    private readonly HotelWebApplicationFactory _factory;

    private HttpClient _client = null!;

    public ChangeTransactionCodeStatusCommandTests(HotelWebApplicationFactory factory)
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
    public async Task ChangeTransactionCodeStatus_WithValidCommand_ReturnsNoContent()
    {
        // Arrange
        var transactionGroupId = await TransactionGroupTestData.CreateTransactionGroupAsync(_client);
        var transactionCodeId = await TransactionCodeTestData.CreateTransactionCodeAsync(_client, transactionGroupId);
        var command = new ChangeTransactionCodeStatusCommand(transactionCodeId, false);

        // Act
        var response = await _client.PutAsJsonAsync("/api/transactioncodes/status", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var getResponse = await _client.GetAsync($"/api/transactioncodes/{transactionCodeId}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task ChangeTransactionCodeStatus_WithNonExistentId_ReturnsNotFound()
    {
        // Arrange
        var command = new ChangeTransactionCodeStatusCommand(Guid.NewGuid(), false);

        // Act
        var response = await _client.PutAsJsonAsync("/api/transactioncodes/status", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}