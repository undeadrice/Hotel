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

public class CreateFolioItemCommandTests : IClassFixture<HotelWebApplicationFactory>, IAsyncLifetime
{
    private readonly HotelWebApplicationFactory _factory;

    private HttpClient _client = null!;

    public CreateFolioItemCommandTests(HotelWebApplicationFactory factory)
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
    public async Task CreateFolioItem_WithChargeTransactionCode_ReturnsItemIdAndAddsCharge()
    {
        // Arrange
        var context = await FiscalAccountTestData.CreateContextAsync(_client, _factory);

        var command = new CreateFolioItemCommand(
            context.MainFolioId,
            "Room service",
            Quantity: 2,
            Amount: 25m,
            context.ChargeTransactionCodeId);

        // Act
        var response = await _client.PostAsJsonAsync("/api/folioitems", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var itemId = await response.Content.ReadFromJsonAsync<Guid>();
        itemId.Should().NotBeEmpty();

        var account = await DataAccess.FiscalAccount(_factory)
            .Include(a => a.Folios)
            .ThenInclude(f => f.Items)
            .SingleAsync(a => a.Id == context.FiscalAccountId);

        var item = account.Folios.Single(f => f.Id == context.MainFolioId)
            .Items.Should().ContainSingle().Which;

        item.Id.Should().Be(itemId);
        item.TransactionType.Should().Be(FolioItemType.Charge);
        item.Amount.Should().Be(25m);
        item.Quantity.Should().Be(2);
    }

    [Fact]
    public async Task CreateFolioItem_WithPaymentTransactionCode_ReturnsItemIdAndAddsPayment()
    {
        // Arrange
        var context = await FiscalAccountTestData.CreateContextAsync(_client, _factory);
        var paymentTransactionCodeId = await FiscalAccountTestData.CreatePaymentTransactionCodeAsync(_client);

        var command = new CreateFolioItemCommand(
            context.MainFolioId,
            "Cash payment",
            Quantity: 1,
            Amount: 150m,
            paymentTransactionCodeId);

        // Act
        var response = await _client.PostAsJsonAsync("/api/folioitems", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var itemId = await response.Content.ReadFromJsonAsync<Guid>();
        itemId.Should().NotBeEmpty();

        var account = await DataAccess.FiscalAccount(_factory)
            .Include(a => a.Folios)
            .ThenInclude(f => f.Items)
            .SingleAsync(a => a.Id == context.FiscalAccountId);

        var item = account.Folios.Single(f => f.Id == context.MainFolioId)
            .Items.Should().ContainSingle().Which;

        item.Id.Should().Be(itemId);
        item.TransactionType.Should().Be(FolioItemType.Payment);
        item.Amount.Should().Be(150m);
        item.Quantity.Should().Be(1);
    }

    [Fact]
    public async Task CreateFolioItem_WithNonExistentFolio_ReturnsNotFound()
    {
        // Arrange
        var context = await FiscalAccountTestData.CreateContextAsync(_client, _factory);

        var command = new CreateFolioItemCommand(
            Guid.NewGuid(),
            "Room service",
            Quantity: 1,
            Amount: 25m,
            context.ChargeTransactionCodeId);

        // Act
        var response = await _client.PostAsJsonAsync("/api/folioitems", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CreateFolioItem_WithNonExistentTransactionCode_ReturnsNotFound()
    {
        // Arrange
        var context = await FiscalAccountTestData.CreateContextAsync(_client, _factory);

        var command = new CreateFolioItemCommand(
            context.MainFolioId,
            "Room service",
            Quantity: 1,
            Amount: 25m,
            Guid.NewGuid());

        // Act
        var response = await _client.PostAsJsonAsync("/api/folioitems", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CreateFolioItem_WithCheckedOutAccount_ReturnsBadRequest()
    {
        // Arrange
        var context = await FiscalAccountTestData.CreateContextAsync(_client, _factory);

        await CheckInReservationAsync(context.ReservationId);
        await SettleMainFolioAsync(context);
        await CheckOutFiscalAccountAsync(context.FiscalAccountId);

        var command = new CreateFolioItemCommand(
            context.MainFolioId,
            "Room service",
            Quantity: 1,
            Amount: 25m,
            context.ChargeTransactionCodeId);

        // Act
        var response = await _client.PostAsJsonAsync("/api/folioitems", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    private async Task CheckInReservationAsync(Guid reservationId)
    {
        var endOfDayResponse = await _client.PostAsync("/api/configurations/end-of-day", null);
        endOfDayResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var checkInResponse = await _client.PostAsync(
            $"/api/reservations/{reservationId}/check-in",
            null);
        checkInResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    private async Task SettleMainFolioAsync(FiscalAccountContext context)
    {
        var chargeResponse = await _client.PostAsync(
            $"/api/fiscalaccounts/{context.ReservationId}/post-room-charge",
            null);
        chargeResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var paymentTransactionCodeId = await FiscalAccountTestData.CreatePaymentTransactionCodeAsync(_client);

        var paymentResponse = await _client.PostAsJsonAsync(
            "/api/folioitems",
            new CreateFolioItemCommand(
                context.MainFolioId,
                "Cash payment",
                Quantity: 1,
                Amount: 100m,
                paymentTransactionCodeId));
        paymentResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var settleResponse = await _client.PostAsJsonAsync(
            "/api/folios/settle",
            new SettleFolioCommand(context.FiscalAccountId, context.MainFolioId));
        settleResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    private async Task CheckOutFiscalAccountAsync(Guid fiscalAccountId)
    {
        var response = await _client.PostAsync(
            $"/api/fiscalaccounts/{fiscalAccountId}/check-out",
            null);
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}
