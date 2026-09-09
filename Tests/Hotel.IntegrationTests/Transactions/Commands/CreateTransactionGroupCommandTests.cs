using FluentAssertions;
using Hotel.Application.Transactions.Commands;
using Hotel.Domain.Transactions.Enums;
using Hotel.IntegrationTests.Infrastructure;
using Hotel.IntegrationTests.Infrastructure.TestData;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Hotel.IntegrationTests.Transactions.Commands;

public class CreateTransactionGroupCommandTests : IClassFixture<HotelWebApplicationFactory>, IAsyncLifetime
{
    private readonly HotelWebApplicationFactory _factory;

    private HttpClient _client = null!;

    public CreateTransactionGroupCommandTests(HotelWebApplicationFactory factory)
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
    public async Task CreateTransactionGroup_WithValidCommand_ReturnsGroupId()
    {
        // Arrange
        var command = new CreateTransactionGroupCommand("CHARGE", "Charge Group", TransactionType.Charge);

        // Act
        var response = await _client.PostAsJsonAsync("/api/transactiongroups", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var transactionGroupId = await response.Content.ReadFromJsonAsync<Guid>();
        transactionGroupId.Should().NotBeEmpty();
    }

    [Fact]
    public async Task CreateTransactionGroup_WithDuplicateCode_ReturnsBadRequest()
    {
        // Arrange
        await TransactionGroupTestData.CreateTransactionGroupAsync(_client, "CHARGE", "Charge Group");

        var command = new CreateTransactionGroupCommand("CHARGE", "Charge Group", TransactionType.Charge);

        // Act
        var response = await _client.PostAsJsonAsync("/api/transactiongroups", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}