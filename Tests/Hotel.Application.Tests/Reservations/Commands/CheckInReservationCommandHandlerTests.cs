using FluentAssertions;
using Hotel.Application.Reservations.Commands;
using Hotel.Domain.Reservations.Entities;
using Hotel.Domain.Reservations.Enums;
using Hotel.Domain.Reservations.Repositories;
using Hotel.Domain.Reservations.Services;
using NSubstitute;
using Xunit;

namespace Hotel.Application.Tests.Reservations.Commands;

public class CheckInReservationCommandHandlerTests
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IRoomAvailabilityService _roomAvailabilityService;
    private readonly CheckInReservationCommandHandler _handler;

    public CheckInReservationCommandHandlerTests()
    {
        _reservationRepository = Substitute.For<IReservationRepository>();
        _roomAvailabilityService = Substitute.For<IRoomAvailabilityService>();
        _handler = new CheckInReservationCommandHandler(_reservationRepository);
    }

    private async Task<Reservation> CreateDueInReservation()
    {
        var reservation = await Reservation.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            "RES-1",
            new DateOnly(2026, 8, 10),
            new DateOnly(2026, 8, 12),
            null,
            [Guid.NewGuid()],
            _roomAvailabilityService);

        reservation.TransitionOnEndOfDay(new DateOnly(2026, 8, 10));

        return reservation;
    }

    [Fact]
    public async Task Handle_ShouldGetReservationAndCheckIn()
    {
        // Arrange
        var reservation = await CreateDueInReservation();

        _reservationRepository
            .GetById(reservation.Id, Arg.Any<CancellationToken>())
            .Returns(reservation);

        var command = new CheckInReservationCommand(reservation.Id);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        reservation.Status.Should().Be(ReservationStatus.InHouse);
        await _reservationRepository.Received(1).GetById(reservation.Id, Arg.Any<CancellationToken>());
    }
}