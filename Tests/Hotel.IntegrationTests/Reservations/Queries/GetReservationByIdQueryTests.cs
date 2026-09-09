using FluentAssertions;
using Hotel.Application.Reservations.TransferObjects;
using Hotel.Domain.Reservations.Enums;
using Hotel.IntegrationTests.Infrastructure;
using Hotel.IntegrationTests.Infrastructure.TestData;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Hotel.IntegrationTests.Reservations.Queries;

public class GetReservationByIdQueryTests : IClassFixture<HotelWebApplicationFactory>, IAsyncLifetime
{
    private readonly HotelWebApplicationFactory _factory;

    private HttpClient _client = null!;

    public GetReservationByIdQueryTests(HotelWebApplicationFactory factory)
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
    public async Task GetReservationById_WithValidId_ReturnsReservation()
    {
        // Arrange
        var context = await ReservationTestData.CreateReservationContextAsync(_client);

        // Act
        var response = await _client.GetAsync($"/api/reservations/{context.ReservationId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var reservation = await response.Content.ReadFromJsonAsync<ReservationDto>();
        reservation.Should().NotBeNull();
        reservation!.Id.Should().Be(context.ReservationId);
        reservation.RoomId.Should().Be(context.RoomId);
        reservation.RatePlanId.Should().Be(context.RatePlanId);
        reservation.CreatorId.Should().Be(context.CreatorId);
        reservation.CycleIdentifier.Should().Be("RES-1");
        reservation.StartDate.Should().Be(RatePlanDates.ValidStartDate);
        reservation.EndDate.Should().Be(RatePlanDates.ValidStartDate.AddDays(2));
        reservation.Status.Should().Be(ReservationStatus.Reserved);
        reservation.GuestIds.Should().ContainSingle().Which.Should().Be(context.CreatorId);
    }

    [Fact]
    public async Task GetReservationById_WithNonExistentId_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync($"/api/reservations/{Guid.NewGuid()}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}