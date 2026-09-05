using FluentAssertions;
using Hotel.Application.Transactions.Commands;
using Hotel.Domain.Transactions.Enums;
using Hotel.IntegrationTests.Infrastructure;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Hotel.IntegrationTests.Transactions.CommandValidations;

public class CreateTransactionGroupCommandValidationTests : IClassFixture<HotelWebApplicationFactory>, IAsyncLifetime
{
    private readonly HotelWebApplicationFactory _factory;

    private HttpClient _client = null!;

    public CreateTransactionGroupCommandValidationTests(HotelWebApplicationFactory factory)
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
    public async Task CreateTransactionGroup_WithEmptyCode_ReturnsBadRequest()
    {
        // Arrange
        var command = new CreateTransactionGroupCommand(string.Empty, "Charge Group", TransactionType.Charge);

        // Act
        var response = await _client.PostAsJsonAsync("/api/transactiongroups", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateTransactionGroup_WithCodeExceedingMaxLength_ReturnsBadRequest()
    {
        // Arrange
        var command = new CreateTransactionGroupCommand(new string('A', 21), "Charge Group", TransactionType.Charge);

        // Act
        var response = await _client.PostAsJsonAsync("/api/transactiongroups", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateTransactionGroup_WithEmptyName_ReturnsBadRequest()
    {
        // Arrange
        var command = new CreateTransactionGroupCommand("CHARGE", string.Empty, TransactionType.Charge);

        // Act
        var response = await _client.PostAsJsonAsync("/api/transactiongroups", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateTransactionGroup_WithNameExceedingMaxLength_ReturnsBadRequest()
    {
        // Arrange
        var command = new CreateTransactionGroupCommand("CHARGE", new string('B', 101), TransactionType.Charge);

        // Act
        var response = await _client.PostAsJsonAsync("/api/transactiongroups", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}