using FluentAssertions;
using Hotel.Application.FiscalAccounting.Commands;
using Hotel.Domain.FiscalAccounting.Entities;
using Hotel.Domain.FiscalAccounting.Enums;
using Hotel.Domain.FiscalAccounting.Repositories;
using Hotel.Domain.Reservations.Entities;
using Hotel.Domain.Reservations.Enums;
using Hotel.Domain.Reservations.Repositories;
using Hotel.Domain.Reservations.Services;
using NSubstitute;
using Xunit;

namespace Hotel.Application.Tests.FiscalAccounting.Commands;

public class CheckOutFiscalAccountCommandHandlerTests
{
    private readonly IFiscalAccountRepository _fiscalAccountRepository;
    private readonly IReservationRepository _reservationRepository;
    private readonly IRoomAvailabilityService _roomAvailabilityService;
    private readonly CheckOutFiscalAccountCommandHandler _handler;

    public CheckOutFiscalAccountCommandHandlerTests()
    {
        _fiscalAccountRepository = Substitute.For<IFiscalAccountRepository>();
        _reservationRepository = Substitute.For<IReservationRepository>();
        _roomAvailabilityService = Substitute.For<IRoomAvailabilityService>();
        _handler = new CheckOutFiscalAccountCommandHandler(_fiscalAccountRepository, _reservationRepository);
    }

    private static FiscalAccount CreateSettledAccount(Guid originatorId)
    {
        var account = FiscalAccount.Create(originatorId, Guid.NewGuid(), "CY-1");
        account.SettleFolio(account.Folios.Single().Id);
        return account;
    }

    private async Task<Reservation> CreateInHouseReservation()
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
        reservation.CheckIn();

        return reservation;
    }

    [Fact]
    public async Task Handle_ShouldCheckOutAccountAndReservation()
    {
        // Arrange
        var reservation = await CreateInHouseReservation();
        var account = CreateSettledAccount(reservation.Id);

        _fiscalAccountRepository
            .GetForCheckOut(account.Id, Arg.Any<CancellationToken>())
            .Returns(account);

        _reservationRepository
            .GetById(account.OriginatorId, Arg.Any<CancellationToken>())
            .Returns(reservation);

        var command = new CheckOutFiscalAccountCommand(account.Id);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        account.Status.Should().Be(FiscalAccountStatus.CheckedOut);
        reservation.Status.Should().Be(ReservationStatus.CheckedOut);

        await _fiscalAccountRepository.Received(1).GetForCheckOut(account.Id, Arg.Any<CancellationToken>());
        await _reservationRepository.Received(1).GetById(account.OriginatorId, Arg.Any<CancellationToken>());
    }
}