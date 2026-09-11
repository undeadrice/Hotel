using FluentAssertions;
using Hotel.Application.Rooming.Commands;
using Hotel.Domain.FiscalAccounting.Enums;
using Hotel.IntegrationTests.Infrastructure;
using Hotel.IntegrationTests.Infrastructure.TestData;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Hotel.IntegrationTests.FiscalAccounting.Commands;

public class PostRoomChargeCommandTests : IClassFixture<HotelWebApplicationFactory>, IAsyncLifetime
{
    private readonly HotelWebApplicationFactory _factory;

    private HttpClient _client = null!;

    public PostRoomChargeCommandTests(HotelWebApplicationFactory factory)
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
    public async Task PostRoomCharge_WithValidReservation_ReturnsItemIdAndAddsCharge()
    {
        // Arrange
        var context = await FiscalAccountTestData.CreateContextAsync(_client, _factory);

        // Act
        var response = await _client.PostAsync(
            $"/api/fiscalaccounts/{context.ReservationId}/post-room-charge",
            null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var itemId = await response.Content.ReadFromJsonAsync<Guid>();
        itemId.Should().NotBeEmpty();

        var account = await DataAccess.FiscalAccount(_factory)
            .Include(a => a.Folios)
            .ThenInclude(f => f.Items)
            .SingleAsync(a => a.Id == context.FiscalAccountId);

        var mainFolio = account.Folios.Single(f => f.Id == context.MainFolioId);

        var item = mainFolio.Items.Should().ContainSingle().Which;
        item.Id.Should().Be(itemId);
        item.TransactionType.Should().Be(FolioItemType.Charge);
        item.Amount.Should().Be(100m);
        item.Quantity.Should().Be(1);
    }

    [Fact]
    public async Task PostRoomCharge_WithNonExistentReservation_ReturnsNotFound()
    {
        // Act
        var response = await _client.PostAsync(
            $"/api/fiscalaccounts/{Guid.NewGuid()}/post-room-charge",
            null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task PostRoomCharge_WithRoomNotInRatePlan_ReturnsBadRequest()
    {
        // Arrange
        var reservationContext = await ReservationTestData.CreateReservationContextAsync(_client);
        var otherRoomTypeId = await RoomTypeTestData.CreateRoomTypeAsync(_client, "Suite");

        var updateRoomResponse = await _client.PutAsJsonAsync(
            "/api/rooms",
            new UpdateRoomCommand(reservationContext.RoomId, "201", otherRoomTypeId));
        updateRoomResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Act
        var response = await _client.PostAsync(
            $"/api/fiscalaccounts/{reservationContext.ReservationId}/post-room-charge",
            null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
