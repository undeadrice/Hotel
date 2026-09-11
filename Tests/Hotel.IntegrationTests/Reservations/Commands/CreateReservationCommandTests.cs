using FluentAssertions;
using Hotel.Application.Reservations.Commands;
using Hotel.Domain.NumberCycles.Enums;
using Hotel.IntegrationTests.Infrastructure;
using Hotel.IntegrationTests.Infrastructure.TestData;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Hotel.IntegrationTests.Reservations.Commands;

public class CreateReservationCommandTests : IClassFixture<HotelWebApplicationFactory>, IAsyncLifetime
{
    private readonly HotelWebApplicationFactory _factory;

    private HttpClient _client = null!;

    public CreateReservationCommandTests(HotelWebApplicationFactory factory)
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
    public async Task CreateReservation_WithValidCommand_ReturnsReservationId()
    {
        // Act
        var reservationId = await ReservationTestData.CreateReservationAsync(_client);

        // Assert
        reservationId.Should().NotBeEmpty();

        var getResponse = await _client.GetAsync($"/api/reservations/{reservationId}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var fiscalAccount = await DataAccess.FiscalAccount(_factory)
            .SingleOrDefaultAsync(a => a.OriginatorId == reservationId);

        fiscalAccount.Should().NotBeNull();
    }

    [Fact]
    public async Task CreateReservation_WithOverlappingDates_ReturnsBadRequest()
    {
        // Arrange
        var context = await ReservationTestData.CreateReservationContextAsync(_client);

        var command = new CreateReservationCommand(
            context.CreatorId,
            context.RoomId,
            context.RatePlanId,
            RatePlanDates.ValidStartDate,
            RatePlanDates.ValidStartDate.AddDays(3),
            null,
            [context.CreatorId]);

        // Act
        var response = await _client.PostAsJsonAsync("/api/reservations", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateReservation_WithStartDateOutsideRatePlan_ReturnsBadRequest()
    {
        // Arrange
        var context = await ReservationTestData.CreateReservationContextAsync(_client);

        var command = new CreateReservationCommand(
            context.CreatorId,
            context.RoomId,
            context.RatePlanId,
            RatePlanDates.ValidStartDate.AddDays(-1),
            RatePlanDates.ValidEndDate,
            null,
            [context.CreatorId]);

        // Act
        var response = await _client.PostAsJsonAsync("/api/reservations", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateReservation_WithEndDateOutsideRatePlan_ReturnsBadRequest()
    {
        // Arrange
        var context = await ReservationTestData.CreateReservationContextAsync(_client);

        var command = new CreateReservationCommand(
            context.CreatorId,
            context.RoomId,
            context.RatePlanId,
            RatePlanDates.ValidStartDate,
            RatePlanDates.ValidEndDate.AddDays(1),
            null,
            [context.CreatorId]);

        // Act
        var response = await _client.PostAsJsonAsync("/api/reservations", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateReservation_WithRoomNotInRatePlan_ReturnsBadRequest()
    {
        // Arrange
        var ratePlanRoomTypeId = await RoomTypeTestData.CreateRoomTypeAsync(_client, "Standard");
        var otherRoomTypeId = await RoomTypeTestData.CreateRoomTypeAsync(_client, "Suite");
        var roomId = await RoomTestData.CreateRoomAsync(_client, "201", otherRoomTypeId);

        var transactionGroupId = await TransactionGroupTestData.CreateTransactionGroupAsync(_client);
        var transactionCodeId = await TransactionCodeTestData.CreateTransactionCodeAsync(_client, transactionGroupId);

        var ratePlanId = await RatePlanTestData.CreateRatePlanAsync(_client, transactionCodeId, ratePlanRoomTypeId);

        await NumberCycleTestData.CreateNumberCycleAsync(_client, NumberCycleTopic.Reservation, "RES", 1);
        await NumberCycleTestData.CreateNumberCycleAsync(_client, NumberCycleTopic.FiscalAccount, "FA", 1);

        var guestId = await GuestTestData.CreateGuestAsync(_client);

        var command = new CreateReservationCommand(
            guestId,
            roomId,
            ratePlanId,
            RatePlanDates.ValidStartDate,
            RatePlanDates.ValidStartDate.AddDays(2),
            null,
            [guestId]);

        // Act
        var response = await _client.PostAsJsonAsync("/api/reservations", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}