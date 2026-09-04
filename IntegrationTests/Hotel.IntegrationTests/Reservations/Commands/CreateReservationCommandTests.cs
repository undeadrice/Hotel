using FluentAssertions;
using Hotel.Application.Reservations.Commands;
using Hotel.IntegrationTests.Infrastructure;
using Hotel.IntegrationTests.Infrastructure.TestData;
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
}