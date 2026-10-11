using FluentAssertions;
using Hotel.Application.FiscalAccounting.Commands;
using Hotel.Domain.Transactions.Enums;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http.Json;

namespace Hotel.IntegrationTests.Infrastructure.TestData;

public record FiscalAccountContext(
    Guid FiscalAccountId,
    Guid MainFolioId,
    Guid ReservationId,
    Guid RatePlanId,
    Guid ChargeTransactionCodeId,
    DateTime CreatedAt);

public static class FiscalAccountTestData
{
    public static async Task<FiscalAccountContext> CreateContextAsync(
        HttpClient client,
        HotelWebApplicationFactory factory)
    {
        var reservationContext = await ReservationTestData.CreateReservationContextAsync(client);

        var account = await DataAccess.FiscalAccount(factory)
            .Include(a => a.Folios)
            .SingleAsync(a => a.OriginatorId == reservationContext.ReservationId);

        var mainFolio = account.Folios.Single(f => f.IsMainFolio);

        var ratePlan = await DataAccess.RatePlan(factory)
            .SingleAsync(rp => rp.Id == reservationContext.RatePlanId);

        return new FiscalAccountContext(
            account.Id,
            mainFolio.Id,
            reservationContext.ReservationId,
            reservationContext.RatePlanId,
            ratePlan.TransactionCodeId,
            account.CreatedAt);
    }

    public static async Task<Guid> CreatePaymentTransactionCodeAsync(HttpClient client)
    {
        var paymentGroupId = await TransactionGroupTestData.CreateTransactionGroupAsync(
            client,
            code: "2001",
            name: "Payments",
            type: TransactionType.Payment);

        return await TransactionCodeTestData.CreateTransactionCodeAsync(
            client,
            paymentGroupId,
            code: "2001",
            name: "Default payment code");
    }

    public static async Task CreateChargeFolioItemAsync(
        HttpClient client,
        FiscalAccountContext context,
        Guid chargeTransactionCodeId,
        string description = "Room charge",
        decimal amount = 100m)
    {
        var response = await client.PostAsJsonAsync(
            "/api/folioitems",
            new CreateFolioItemCommand(
                context.MainFolioId,
                description,
                Quantity: 1,
                amount,
                chargeTransactionCodeId));
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    public static async Task CreatePaymentFolioItemAsync(
        HttpClient client,
        FiscalAccountContext context,
        Guid paymentTransactionCodeId,
        string description = "Cash payment",
        decimal amount = 100m)
    {
        var response = await client.PostAsJsonAsync(
            "/api/folioitems",
            new CreateFolioItemCommand(
                context.MainFolioId,
                description,
                Quantity: 1,
                amount,
                paymentTransactionCodeId));
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    public static async Task PostRoomChargeAsync(
        HttpClient client,
        FiscalAccountContext context)
    {
        var response = await client.PostAsync(
            $"/api/fiscalaccounts/{context.ReservationId}/post-room-charge",
            null);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    public static async Task SettleFolioAsync(
        HttpClient client,
        FiscalAccountContext context)
    {
        var settleResponse = await client.PostAsJsonAsync(
            "/api/folios/settle",
            new SettleFolioCommand(context.FiscalAccountId, context.MainFolioId));
        settleResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    public static async Task SettleMainFolioAsync(
        HttpClient client,
        FiscalAccountContext context)
    {
        await PostRoomChargeAsync(client, context);

        var paymentTransactionCodeId = await CreatePaymentTransactionCodeAsync(client);

        await CreatePaymentFolioItemAsync(client, context, paymentTransactionCodeId);
        await SettleFolioAsync(client, context);
    }
}
