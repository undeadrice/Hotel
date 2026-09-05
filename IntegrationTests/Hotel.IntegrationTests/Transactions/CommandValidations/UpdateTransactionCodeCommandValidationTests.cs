using FluentAssertions;
using Hotel.Application.Transactions.Commands;
using Hotel.IntegrationTests.Infrastructure;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Hotel.IntegrationTests.Transactions.CommandValidations;

public class UpdateTransactionCodeCommandValidationTests : IClassFixture<HotelWebApplicationFactory>, IAsyncLifetime
{
    private readonly HotelWebApplicationFactory _factory;

    private HttpClient _client = null!;

    public UpdateTransactionCodeCommandValidationTests(HotelWebApplicationFactory factory)
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
    public async Task UpdateTransactionCode_WithEmptyId_ReturnsBadRequest()
    {
        // Arrange
        var command = new UpdateTransactionCodeCommand(Guid.Empty, Guid.NewGuid(), "ROOM", "Room Charge");

        // Act
        var response = await _client.PutAsJsonAsync("/api/transactioncodes", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateTransactionCode_WithEmptyTransactionGroupId_ReturnsBadRequest()
    {
        // Arrange
        var command = new UpdateTransactionCodeCommand(Guid.NewGuid(), Guid.Empty, "ROOM", "Room Charge");

        // Act
        var response = await _client.PutAsJsonAsync("/api/transactioncodes", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateTransactionCode_WithEmptyCode_ReturnsBadRequest()
    {
        // Arrange
        var command = new UpdateTransactionCodeCommand(Guid.NewGuid(), Guid.NewGuid(), string.Empty, "Room Charge");

        // Act
        var response = await _client.PutAsJsonAsync("/api/transactioncodes", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateTransactionCode_WithCodeExceedingMaxLength_ReturnsBadRequest()
    {
        // Arrange
        var command = new UpdateTransactionCodeCommand(Guid.NewGuid(), Guid.NewGuid(), new string('A', 21), "Room Charge");

        // Act
        var response = await _client.PutAsJsonAsync("/api/transactioncodes", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateTransactionCode_WithEmptyName_ReturnsBadRequest()
    {
        // Arrange
        var command = new UpdateTransactionCodeCommand(Guid.NewGuid(), Guid.NewGuid(), "ROOM", string.Empty);

        // Act
        var response = await _client.PutAsJsonAsync("/api/transactioncodes", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateTransactionCode_WithNameExceedingMaxLength_ReturnsBadRequest()
    {
        // Arrange
        var command = new UpdateTransactionCodeCommand(Guid.NewGuid(), Guid.NewGuid(), "ROOM", new string('B', 101));

        // Act
        var response = await _client.PutAsJsonAsync("/api/transactioncodes", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}