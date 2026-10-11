using FluentAssertions;
using Hotel.Application.Reservations.TransferObjects;
using Hotel.Domain.Reservations.Enums;
using Hotel.IntegrationTests.Infrastructure;
using Hotel.IntegrationTests.Infrastructure.TestData;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Hotel.IntegrationTests.Reservations.Queries;

public class GetReservationsQueryTests : IClassFixture<HotelWebApplicationFactory>, IAsyncLifetime
{
    private readonly HotelWebApplicationFactory _factory;

    private HttpClient _client = null!;

    public GetReservationsQueryTests(HotelWebApplicationFactory factory)
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
    public async Task GetReservations_WhenNoReservationsExist_ReturnsEmptyList()
    {
        // Act
        var response = await _client.GetAsync("/api/reservations");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var reservations = await response.Content.ReadFromJsonAsync<List<ReservationListDto>>();
        reservations.Should().NotBeNull();
        reservations.Should().BeEmpty();
    }

    [Fact]
    public async Task GetReservations_AfterCreatingReservation_ReturnsReservation()
    {
        // Arrange
        var context = await ReservationTestData.CreateReservationContextAsync(_client);

        // Act
        var response = await _client.GetAsync("/api/reservations");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var reservations = await response.Content.ReadFromJsonAsync<List<ReservationListDto>>();
        reservations.Should().ContainSingle(r => r.Id == context.ReservationId);

        var reservation = reservations!.Single(r => r.Id == context.ReservationId);
        reservation.CycleIdentifier.Should().Be("RES-1");
        reservation.RoomName.Should().Be("101");
        reservation.RatePlanName.Should().Be("Standard");
        reservation.CreatorName.Should().Be("John Doe");
        reservation.StartDate.Should().Be(RatePlanDates.ValidStartDate);
        reservation.EndDate.Should().Be(RatePlanDates.ValidStartDate.AddDays(2));
        reservation.Status.Should().Be(ReservationStatus.Reserved);
        reservation.GuestCount.Should().Be(1);
    }
}