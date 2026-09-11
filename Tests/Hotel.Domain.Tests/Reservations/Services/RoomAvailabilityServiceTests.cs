using FluentAssertions;
using Hotel.Domain.Reservations.Repositories;
using Hotel.Domain.Reservations.Services;
using NSubstitute;
using Xunit;

namespace Hotel.Domain.Tests.Reservations.Services;

public class RoomAvailabilityServiceTests
{
    private readonly IReservationRepository _reservationRepository = Substitute.For<IReservationRepository>();
    private readonly RoomAvailabilityService _service;

    public RoomAvailabilityServiceTests()
    {
        _service = new RoomAvailabilityService(_reservationRepository);
    }

    [Fact]
    public async Task IsRoomOccupied_WhenRepositoryReturnsTrue_ShouldReturnTrue()
    {
        // Arrange
        var roomId = Guid.NewGuid();
        var startDate = new DateOnly(2026, 8, 10);
        var endDate = new DateOnly(2026, 8, 12);

        _reservationRepository.HasOverlappingReservation(roomId, startDate, endDate, Arg.Any<CancellationToken>())
            .Returns(true);

        // Act
        var result = await _service.IsRoomOccupied(roomId, startDate, endDate);

        // Assert
        result.Should().BeTrue();
        await _reservationRepository.Received(1)
            .HasOverlappingReservation(roomId, startDate, endDate, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task IsRoomOccupied_WhenRepositoryReturnsFalse_ShouldReturnFalse()
    {
        // Arrange
        var roomId = Guid.NewGuid();
        var startDate = new DateOnly(2026, 8, 10);
        var endDate = new DateOnly(2026, 8, 12);

        _reservationRepository.HasOverlappingReservation(roomId, startDate, endDate, Arg.Any<CancellationToken>())
            .Returns(false);

        // Act
        var result = await _service.IsRoomOccupied(roomId, startDate, endDate);

        // Assert
        result.Should().BeFalse();
        await _reservationRepository.Received(1)
            .HasOverlappingReservation(roomId, startDate, endDate, Arg.Any<CancellationToken>());
    }
}