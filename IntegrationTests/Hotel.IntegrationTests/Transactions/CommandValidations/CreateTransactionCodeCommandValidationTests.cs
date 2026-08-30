using FluentAssertions;
using Hotel.Application.Transactions.Commands;
using Hotel.IntegrationTests.Infrastructure;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Hotel.IntegrationTests.Transactions.CommandValidations;

public class CreateTransactionCodeCommandValidationTests : IClassFixture<HotelWebApplicationFactory>, IAsyncLifetime
{
    private readonly HotelWebApplicationFactory _factory;

    private HttpClient _client = null!;

    public CreateTransactionCodeCommandValidationTests(HotelWebApplicationFactory factory)
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
    public async Task CreateTransactionCode_WithEmptyTransactionGroupId_ReturnsBadRequest()
    {
        // Arrange
        var command = new CreateTransactionCodeCommand(Guid.Empty, "ROOM", "Room Charge");

        // Act
        var response = await _client.PostAsJsonAsync("/api/transactioncodes", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateTransactionCode_WithEmptyCode_ReturnsBadRequest()
    {
        // Arrange
        var command = new CreateTransactionCodeCommand(Guid.NewGuid(), string.Empty, "Room Charge");

        // Act
        var response = await _client.PostAsJsonAsync("/api/transactioncodes", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateTransactionCode_WithCodeExceedingMaxLength_ReturnsBadRequest()
    {
        // Arrange
        var command = new CreateTransactionCodeCommand(Guid.NewGuid(), new string('A', 21), "Room Charge");

        // Act
        var response = await _client.PostAsJsonAsync("/api/transactioncodes", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateTransactionCode_WithEmptyName_ReturnsBadRequest()
    {
        // Arrange
        var command = new CreateTransactionCodeCommand(Guid.NewGuid(), "ROOM", string.Empty);

        // Act
        var response = await _client.PostAsJsonAsync("/api/transactioncodes", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateTransactionCode_WithNameExceedingMaxLength_ReturnsBadRequest()
    {
        // Arrange
        var command = new CreateTransactionCodeCommand(Guid.NewGuid(), "ROOM", new string('B', 101));

        // Act
        var response = await _client.PostAsJsonAsync("/api/transactioncodes", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}