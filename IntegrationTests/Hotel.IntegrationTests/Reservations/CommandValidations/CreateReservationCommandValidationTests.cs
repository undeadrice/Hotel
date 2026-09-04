using FluentAssertions;
using Hotel.Application.Reservations.Commands;
using Hotel.IntegrationTests.Infrastructure;
using Hotel.IntegrationTests.Infrastructure.TestData;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Hotel.IntegrationTests.Reservations.CommandValidations;

public class CreateReservationCommandValidationTests : IClassFixture<HotelWebApplicationFactory>, IAsyncLifetime
{
    private readonly HotelWebApplicationFactory _factory;

    private HttpClient _client = null!;

    public CreateReservationCommandValidationTests(HotelWebApplicationFactory factory)
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
    public async Task CreateReservation_WithEmptyCreatorId_ReturnsBadRequest()
    {
        // Arrange
        var command = new CreateReservationCommand(
            Guid.Empty,
            Guid.NewGuid(),
            Guid.NewGuid(),
            RatePlanDates.ValidStartDate,
            RatePlanDates.ValidStartDate.AddDays(2),
            null,
            [Guid.NewGuid()]);

        // Act
        var response = await _client.PostAsJsonAsync("/api/reservations", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateReservation_WithEmptyRoomId_ReturnsBadRequest()
    {
        // Arrange
        var command = new CreateReservationCommand(
            Guid.NewGuid(),
            Guid.Empty,
            Guid.NewGuid(),
            RatePlanDates.ValidStartDate,
            RatePlanDates.ValidStartDate.AddDays(2),
            null,
            [Guid.NewGuid()]);

        // Act
        var response = await _client.PostAsJsonAsync("/api/reservations", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateReservation_WithEmptyRatePlanId_ReturnsBadRequest()
    {
        // Arrange
        var command = new CreateReservationCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.Empty,
            RatePlanDates.ValidStartDate,
            RatePlanDates.ValidStartDate.AddDays(2),
            null,
            [Guid.NewGuid()]);

        // Act
        var response = await _client.PostAsJsonAsync("/api/reservations", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateReservation_WithStartDateAfterEndDate_ReturnsBadRequest()
    {
        // Arrange
        var command = new CreateReservationCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            RatePlanDates.ValidStartDate.AddDays(2),
            RatePlanDates.ValidStartDate,
            null,
            [Guid.NewGuid()]);

        // Act
        var response = await _client.PostAsJsonAsync("/api/reservations", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateReservation_WithEmptyGuestsList_ReturnsBadRequest()
    {
        // Arrange
        var command = new CreateReservationCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            RatePlanDates.ValidStartDate,
            RatePlanDates.ValidStartDate.AddDays(2),
            null,
            []);

        // Act
        var response = await _client.PostAsJsonAsync("/api/reservations", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}