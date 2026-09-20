using FluentAssertions;
using Hotel.Application.FiscalAccounting.TransferObjects;
using Hotel.Domain.FiscalAccounting.Enums;
using Hotel.IntegrationTests.Infrastructure;
using Hotel.IntegrationTests.Infrastructure.TestData;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Hotel.IntegrationTests.FiscalAccounting.Queries;

public class GetFiscalAccountsQueryTests : IClassFixture<HotelWebApplicationFactory>, IAsyncLifetime
{
    private readonly HotelWebApplicationFactory _factory;

    private HttpClient _client = null!;

    public GetFiscalAccountsQueryTests(HotelWebApplicationFactory factory)
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
    public async Task GetFiscalAccounts_WithExistingAccounts_ReturnsAccounts()
    {
        // Arrange
        var context = await FiscalAccountTestData.CreateContextAsync(_client, _factory);

        // Act
        var response = await _client.GetAsync("/api/fiscalaccounts");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var accounts = await response.Content.ReadFromJsonAsync<IReadOnlyCollection<FiscalAccountListItemDto>>();
        accounts.Should().NotBeNull();

        var account = accounts.Should().ContainSingle(a => a.Id == context.FiscalAccountId).Which;
        account.CycleIdentifier.Should().Be("FA-1");
        account.OwnerFullName.Should().Be("John Doe");
        account.Status.Should().Be(FiscalAccountStatus.Open);
        account.CreatedAt.Date.Should().Be(context.CreatedAt.Date);
    }

    [Fact]
    public async Task GetFiscalAccounts_WithNoAccounts_ReturnsEmptyCollection()
    {
        // Act
        var response = await _client.GetAsync("/api/fiscalaccounts");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var accounts = await response.Content.ReadFromJsonAsync<IReadOnlyCollection<FiscalAccountListItemDto>>();
        accounts.Should().NotBeNull();
        accounts.Should().BeEmpty();
    }
}