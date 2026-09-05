using FluentAssertions;
using Hotel.Application.Transactions.Commands;
using Hotel.Domain.Transactions.Enums;
using Hotel.IntegrationTests.Infrastructure;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Hotel.IntegrationTests.Transactions.CommandValidations;

public class UpdateTransactionGroupCommandValidationTests : IClassFixture<HotelWebApplicationFactory>, IAsyncLifetime
{
    private readonly HotelWebApplicationFactory _factory;

    private HttpClient _client = null!;

    public UpdateTransactionGroupCommandValidationTests(HotelWebApplicationFactory factory)
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
    public async Task UpdateTransactionGroup_WithEmptyId_ReturnsBadRequest()
    {
        // Arrange
        var command = new UpdateTransactionGroupCommand(Guid.Empty, "CHARGE", "Charge Group", TransactionType.Charge);

        // Act
        var response = await _client.PutAsJsonAsync("/api/transactiongroups", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateTransactionGroup_WithEmptyCode_ReturnsBadRequest()
    {
        // Arrange
        var command = new UpdateTransactionGroupCommand(Guid.NewGuid(), string.Empty, "Charge Group", TransactionType.Charge);

        // Act
        var response = await _client.PutAsJsonAsync("/api/transactiongroups", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateTransactionGroup_WithCodeExceedingMaxLength_ReturnsBadRequest()
    {
        // Arrange
        var command = new UpdateTransactionGroupCommand(Guid.NewGuid(), new string('A', 21), "Charge Group", TransactionType.Charge);

        // Act
        var response = await _client.PutAsJsonAsync("/api/transactiongroups", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateTransactionGroup_WithEmptyName_ReturnsBadRequest()
    {
        // Arrange
        var command = new UpdateTransactionGroupCommand(Guid.NewGuid(), "CHARGE", string.Empty, TransactionType.Charge);

        // Act
        var response = await _client.PutAsJsonAsync("/api/transactiongroups", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateTransactionGroup_WithNameExceedingMaxLength_ReturnsBadRequest()
    {
        // Arrange
        var command = new UpdateTransactionGroupCommand(Guid.NewGuid(), "CHARGE", new string('B', 101), TransactionType.Charge);

        // Act
        var response = await _client.PutAsJsonAsync("/api/transactiongroups", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}