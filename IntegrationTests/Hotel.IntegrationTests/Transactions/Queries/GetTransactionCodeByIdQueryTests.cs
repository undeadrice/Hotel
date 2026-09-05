using FluentAssertions;
using Hotel.Application.Transactions.TransferObjects;
using Hotel.IntegrationTests.Infrastructure;
using Hotel.IntegrationTests.Infrastructure.TestData;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Hotel.IntegrationTests.Transactions.Queries;

public class GetTransactionCodeByIdQueryTests : IClassFixture<HotelWebApplicationFactory>, IAsyncLifetime
{
    private readonly HotelWebApplicationFactory _factory;

    private HttpClient _client = null!;

    public GetTransactionCodeByIdQueryTests(HotelWebApplicationFactory factory)
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
    public async Task GetTransactionCodeById_WithExistingCode_ReturnsCode()
    {
        // Arrange
        var transactionGroupId = await TransactionGroupTestData.CreateTransactionGroupAsync(_client, "CHARGE", "Charge Group");
        var transactionCodeId = await TransactionCodeTestData.CreateTransactionCodeAsync(_client, transactionGroupId, "ROOM", "Room Charge");

        // Act
        var response = await _client.GetAsync($"/api/transactioncodes/{transactionCodeId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var transactionCode = await response.Content.ReadFromJsonAsync<TransactionCodeDto>();
        transactionCode.Should().NotBeNull();
        transactionCode!.Id.Should().Be(transactionCodeId);
        transactionCode.Code.Should().Be("ROOM");
        transactionCode.Name.Should().Be("Room Charge");
        transactionCode.TransactionGroupId.Should().Be(transactionGroupId);
        transactionCode.TransactionGroupName.Should().Be("Charge Group");
        transactionCode.IsActive.Should().BeTrue();
    }

    [Fact]
    public async Task GetTransactionCodeById_WithNonExistentId_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync($"/api/transactioncodes/{Guid.NewGuid()}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}