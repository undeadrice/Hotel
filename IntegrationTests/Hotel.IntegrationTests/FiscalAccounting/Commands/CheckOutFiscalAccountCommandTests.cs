using FluentAssertions;
using Hotel.Domain.FiscalAccounting.Enums;
using Hotel.Domain.Reservations.Enums;
using Hotel.IntegrationTests.Infrastructure;
using Hotel.IntegrationTests.Infrastructure.TestData;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Xunit;

namespace Hotel.IntegrationTests.FiscalAccounting.Commands;

public class CheckOutFiscalAccountCommandTests : IClassFixture<HotelWebApplicationFactory>, IAsyncLifetime
{
    private readonly HotelWebApplicationFactory _factory;

    private HttpClient _client = null!;

    public CheckOutFiscalAccountCommandTests(HotelWebApplicationFactory factory)
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
    public async Task CheckOut_WithSettledFolioAndInHouseReservation_ReturnsNoContent()
    {
        // Arrange
        var context = await FiscalAccountTestData.CreateContextAsync(_client, _factory);

        await EndOfDayTestData.RunEndOfDayAsync(_client);
        await ReservationTestData.CheckInReservationAsync(_client, context.ReservationId);
        await FiscalAccountTestData.SettleMainFolioAsync(_client, context);

        // Act
        var response = await _client.PostAsync(
            $"/api/fiscalaccounts/{context.FiscalAccountId}/check-out",
            null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var account = await DataAccess.FiscalAccount(_factory)
            .SingleAsync(a => a.Id == context.FiscalAccountId);
        account.Status.Should().Be(FiscalAccountStatus.CheckedOut);

        var reservation = await DataAccess.Reservation(_factory)
            .SingleAsync(r => r.Id == context.ReservationId);
        reservation.Status.Should().Be(ReservationStatus.CheckedOut);
    }

    [Fact]
    public async Task CheckOut_WithUnsettledFolio_ReturnsBadRequest()
    {
        // Arrange
        var context = await FiscalAccountTestData.CreateContextAsync(_client, _factory);
        await EndOfDayTestData.RunEndOfDayAsync(_client);
        await ReservationTestData.CheckInReservationAsync(_client, context.ReservationId);

        // Act
        var response = await _client.PostAsync(
            $"/api/fiscalaccounts/{context.FiscalAccountId}/check-out",
            null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CheckOut_WhenReservationNotInHouse_ReturnsBadRequest()
    {
        // Arrange
        var context = await FiscalAccountTestData.CreateContextAsync(_client, _factory);

        await FiscalAccountTestData.SettleFolioAsync(_client, context);

        // Act
        var response = await _client.PostAsync(
            $"/api/fiscalaccounts/{context.FiscalAccountId}/check-out",
            null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
