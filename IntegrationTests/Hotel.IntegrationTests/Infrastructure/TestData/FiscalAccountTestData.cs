using Hotel.Domain.Transactions.Enums;
using Microsoft.EntityFrameworkCore;

namespace Hotel.IntegrationTests.Infrastructure.TestData;

public record FiscalAccountContext(
    Guid FiscalAccountId,
    Guid MainFolioId,
    Guid ReservationId,
    Guid RatePlanId,
    Guid ChargeTransactionCodeId);

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
            ratePlan.TransactionCodeId);
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
}