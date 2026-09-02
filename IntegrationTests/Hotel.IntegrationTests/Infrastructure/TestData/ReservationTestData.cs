using FluentAssertions;
using Hotel.Application.Reservations.Commands;
using Hotel.Domain.NumberCycles.Enums;
using System.Net;
using System.Net.Http.Json;

namespace Hotel.IntegrationTests.Infrastructure.TestData;

public record ReservationContext(
    Guid ReservationId,
    Guid RoomId,
    Guid RatePlanId,
    Guid CreatorId);

public static class ReservationTestData
{
    public static async Task<ReservationContext> CreateReservationContextAsync(
        HttpClient client,
        DateOnly? startDate = null,
        DateOnly? endDate = null)
    {
        var roomTypeId = await RoomTypeTestData.CreateRoomTypeAsync(client);
        var roomId = await RoomTestData.CreateRoomAsync(client, "101", roomTypeId);

        var transactionGroupId = await TransactionGroupTestData.CreateTransactionGroupAsync(client);
        var transactionCodeId = await TransactionCodeTestData.CreateTransactionCodeAsync(client, transactionGroupId);

        var ratePlanId = await RatePlanTestData.CreateRatePlanAsync(client, transactionCodeId, roomTypeId);

        await NumberCycleTestData.CreateNumberCycleAsync(client, NumberCycleTopic.Reservation, "RES", 1);
        await NumberCycleTestData.CreateNumberCycleAsync(client, NumberCycleTopic.FiscalAccount, "FA", 1);

        var guestId = await GuestTestData.CreateGuestAsync(client);

        var start = startDate ?? new DateOnly(2026, 8, 10);
        var end = endDate ?? new DateOnly(2026, 8, 12);

        var response = await client.PostAsJsonAsync(
            "/api/reservations",
            new CreateReservationCommand(
                guestId,
                roomId,
                ratePlanId,
                start,
                end,
                null,
                [guestId]));

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var reservationId = await response.Content.ReadFromJsonAsync<Guid>();

        return new ReservationContext(reservationId, roomId, ratePlanId, guestId);
    }

    public static async Task<Guid> CreateReservationAsync(
        HttpClient client,
        DateOnly? startDate = null,
        DateOnly? endDate = null)
    {
        var context = await CreateReservationContextAsync(client, startDate, endDate);
        return context.ReservationId;
    }
}