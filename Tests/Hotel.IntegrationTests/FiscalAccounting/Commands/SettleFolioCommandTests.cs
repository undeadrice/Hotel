using FluentAssertions;
using Hotel.Application.FiscalAccounting.Commands;
using Hotel.Domain.FiscalAccounting.Enums;
using Hotel.IntegrationTests.Infrastructure;
using Hotel.IntegrationTests.Infrastructure.TestData;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Hotel.IntegrationTests.FiscalAccounting.Commands;

public class SettleFolioCommandTests : IClassFixture<HotelWebApplicationFactory>, IAsyncLifetime
{
    private readonly HotelWebApplicationFactory _factory;

    private HttpClient _client = null!;

    public SettleFolioCommandTests(HotelWebApplicationFactory factory)
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
    public async Task SettleFolio_WithBalancedFolio_ReturnsNoContentAndSettlesFolio()
    {
        // Arrange
        var context = await FiscalAccountTestData.CreateContextAsync(_client, _factory);
        var paymentTransactionCodeId = await FiscalAccountTestData.CreatePaymentTransactionCodeAsync(_client);

        await FiscalAccountTestData.CreateChargeFolioItemAsync(
            _client,
            context,
            context.ChargeTransactionCodeId);
        await FiscalAccountTestData.CreatePaymentFolioItemAsync(
            _client,
            context,
            paymentTransactionCodeId);

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/folios/settle",
            new SettleFolioCommand(context.FiscalAccountId, context.MainFolioId));

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var account = await DataAccess.FiscalAccount(_factory)
            .Include(a => a.Folios)
            .SingleAsync(a => a.Id == context.FiscalAccountId);

        account.Folios.Single(f => f.Id == context.MainFolioId)
            .Status.Should().Be(FolioStatus.Settled);
    }

    [Fact]
    public async Task SettleFolio_WithUnbalancedFolio_ReturnsBadRequest()
    {
        // Arrange
        var context = await FiscalAccountTestData.CreateContextAsync(_client, _factory);

        await FiscalAccountTestData.CreateChargeFolioItemAsync(
            _client,
            context,
            context.ChargeTransactionCodeId);

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/folios/settle",
            new SettleFolioCommand(context.FiscalAccountId, context.MainFolioId));

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task SettleFolio_WithAlreadySettledFolio_ReturnsBadRequest()
    {
        // Arrange
        var context = await FiscalAccountTestData.CreateContextAsync(_client, _factory);
        var paymentTransactionCodeId = await FiscalAccountTestData.CreatePaymentTransactionCodeAsync(_client);

        await FiscalAccountTestData.CreateChargeFolioItemAsync(
            _client,
            context,
            context.ChargeTransactionCodeId);
        await FiscalAccountTestData.CreatePaymentFolioItemAsync(
            _client,
            context,
            paymentTransactionCodeId);

        var firstSettleResponse = await _client.PostAsJsonAsync(
            "/api/folios/settle",
            new SettleFolioCommand(context.FiscalAccountId, context.MainFolioId));
        firstSettleResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/folios/settle",
            new SettleFolioCommand(context.FiscalAccountId, context.MainFolioId));

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
