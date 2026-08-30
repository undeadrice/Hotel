using FluentAssertions;
using Hotel.Application.Transactions.Commands;
using Hotel.IntegrationTests.Infrastructure;
using Hotel.IntegrationTests.Infrastructure.TestData;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Hotel.IntegrationTests.Transactions.Commands;

public class CreateTransactionCodeCommandTests : IClassFixture<HotelWebApplicationFactory>, IAsyncLifetime
{
    private readonly HotelWebApplicationFactory _factory;

    private HttpClient _client = null!;

    public CreateTransactionCodeCommandTests(HotelWebApplicationFactory factory)
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
    public async Task CreateTransactionCode_WithValidCommand_ReturnsCodeId()
    {
        // Arrange
        var transactionGroupId = await TransactionGroupTestData.CreateTransactionGroupAsync(_client);
        var command = new CreateTransactionCodeCommand(transactionGroupId, "ROOM", "Room Charge");

        // Act
        var response = await _client.PostAsJsonAsync("/api/transactioncodes", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var transactionCodeId = await response.Content.ReadFromJsonAsync<Guid>();
        transactionCodeId.Should().NotBeEmpty();
    }

    [Fact]
    public async Task CreateTransactionCode_WithDuplicateCode_ReturnsBadRequest()
    {
        // Arrange
        var transactionGroupId = await TransactionGroupTestData.CreateTransactionGroupAsync(_client);
        await TransactionCodeTestData.CreateTransactionCodeAsync(_client, transactionGroupId, "ROOM", "Room Charge");

        var command = new CreateTransactionCodeCommand(transactionGroupId, "ROOM", "Another Room Charge");

        // Act
        var response = await _client.PostAsJsonAsync("/api/transactioncodes", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateTransactionCode_WithNonExistentGroup_ReturnsNotFound()
    {
        // Arrange
        var command = new CreateTransactionCodeCommand(Guid.NewGuid(), "ROOM", "Room Charge");

        // Act
        var response = await _client.PostAsJsonAsync("/api/transactioncodes", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}