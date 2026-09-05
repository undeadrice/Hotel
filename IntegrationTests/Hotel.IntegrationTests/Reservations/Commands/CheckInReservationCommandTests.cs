using FluentAssertions;
using Hotel.Application.Reservations.TransferObjects;
using Hotel.Domain.Reservations.Enums;
using Hotel.IntegrationTests.Infrastructure;
using Hotel.IntegrationTests.Infrastructure.TestData;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Hotel.IntegrationTests.Reservations.Commands;

public class CheckInReservationCommandTests : IClassFixture<HotelWebApplicationFactory>, IAsyncLifetime
{
    private readonly HotelWebApplicationFactory _factory;

    private HttpClient _client = null!;

    public CheckInReservationCommandTests(HotelWebApplicationFactory factory)
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
    public async Task CheckInReservation_WhenDueIn_ReturnsNoContentAndTransitionsToInHouse()
    {
        // Arrange
        var reservationId = await ReservationTestData.CreateReservationAsync(_client);

        // Run end of day to move the reservation from Reserved to DueIn.
        var endOfDayResponse = await _client.PostAsync("/api/configurations/end-of-day", null);
        endOfDayResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        // Act
        var response = await _client.PostAsync($"/api/reservations/{reservationId}/check-in", null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var getResponse = await _client.GetAsync($"/api/reservations/{reservationId}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var reservation = await getResponse.Content.ReadFromJsonAsync<ReservationDto>();
        reservation!.Status.Should().Be(ReservationStatus.InHouse);
    }

    [Fact]
    public async Task CheckInReservation_WhenReserved_ReturnsBadRequest()
    {
        // Arrange
        var reservationId = await ReservationTestData.CreateReservationAsync(_client);

        // Act
        var response = await _client.PostAsync($"/api/reservations/{reservationId}/check-in", null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}