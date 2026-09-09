using FluentAssertions;
using Hotel.Application.Transactions.TransferObjects;
using Hotel.Domain.Transactions.Enums;
using Hotel.IntegrationTests.Infrastructure;
using Hotel.IntegrationTests.Infrastructure.TestData;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Hotel.IntegrationTests.Transactions.Queries;

public class GetTransactionGroupByIdQueryTests : IClassFixture<HotelWebApplicationFactory>, IAsyncLifetime
{
    private readonly HotelWebApplicationFactory _factory;

    private HttpClient _client = null!;

    public GetTransactionGroupByIdQueryTests(HotelWebApplicationFactory factory)
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
    public async Task GetTransactionGroupById_WithExistingGroup_ReturnsGroup()
    {
        // Arrange
        var transactionGroupId = await TransactionGroupTestData.CreateTransactionGroupAsync(_client, "CHARGE", "Charge Group");

        // Act
        var response = await _client.GetAsync($"/api/transactiongroups/{transactionGroupId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var transactionGroup = await response.Content.ReadFromJsonAsync<TransactionGroupDto>();
        transactionGroup.Should().NotBeNull();
        transactionGroup!.Id.Should().Be(transactionGroupId);
        transactionGroup.Code.Should().Be("CHARGE");
        transactionGroup.Name.Should().Be("Charge Group");
        transactionGroup.Type.Should().Be(TransactionType.Charge);
        transactionGroup.IsActive.Should().BeTrue();
    }

    [Fact]
    public async Task GetTransactionGroupById_WithNonExistentId_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync($"/api/transactiongroups/{Guid.NewGuid()}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}