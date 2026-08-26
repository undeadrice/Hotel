using FluentAssertions;
using Hotel.Application.Configurations.Services;
using Hotel.Application.FiscalAccounting.Commands;
using Hotel.Domain.FiscalAccounting.Entities;
using Hotel.Domain.FiscalAccounting.Enums;
using Hotel.Domain.FiscalAccounting.Repositories;
using Hotel.Domain.RatePlans.Entities;
using Hotel.Domain.RatePlans.Repositories;
using Hotel.Domain.Reservations.Entities;
using Hotel.Domain.Reservations.Exceptions;
using Hotel.Domain.Reservations.Repositories;
using Hotel.Domain.Reservations.Services;
using Hotel.Domain.Rooming.Entities;
using Hotel.Domain.Rooming.Repositories;
using NSubstitute;
using Xunit;

namespace Hotel.Application.Tests.FiscalAccounting.Commands;

public class PostRoomChargeCommandHandlerTests
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IRatePlanRepository _ratePlanRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IFiscalAccountRepository _fiscalAccountRepository;
    private readonly IBusinessDateProvider _businessDateProvider;
    private readonly IRoomAvailabilityService _roomAvailabilityService;
    private readonly PostRoomChargeCommandHandler _handler;

    public PostRoomChargeCommandHandlerTests()
    {
        _reservationRepository = Substitute.For<IReservationRepository>();
        _ratePlanRepository = Substitute.For<IRatePlanRepository>();
        _roomRepository = Substitute.For<IRoomRepository>();
        _fiscalAccountRepository = Substitute.For<IFiscalAccountRepository>();
        _businessDateProvider = Substitute.For<IBusinessDateProvider>();
        _roomAvailabilityService = Substitute.For<IRoomAvailabilityService>();
        _handler = new PostRoomChargeCommandHandler(
            _reservationRepository,
            _ratePlanRepository,
            _roomRepository,
            _fiscalAccountRepository,
            _businessDateProvider);
    }

    [Fact]
    public async Task Handle_ShouldPostChargeToMainFolio()
    {
        // Arrange
        var roomTypeId = Guid.NewGuid();

        var ratePlan = RatePlan.Create(
            "Standard",
            Guid.NewGuid(),
            new DateOnly(2026, 1, 1),
            new DateOnly(2026, 12, 31),
            [new RoomTypePriceDefinition(roomTypeId, 150m)]);

        var room = Room.Create("101", roomTypeId);

        var reservation = await Reservation.Create(
            Guid.NewGuid(),
            room.Id,
            ratePlan.Id,
            "RES-1",
            new DateOnly(2026, 8, 10),
            new DateOnly(2026, 8, 12),
            null,
            [Guid.NewGuid()],
            _roomAvailabilityService);

        var fiscalAccount = FiscalAccount.Create(reservation.Id, Guid.NewGuid(), "CY-1");

        _reservationRepository.GetById(reservation.Id, Arg.Any<CancellationToken>()).Returns(reservation);
        _ratePlanRepository.GetById(ratePlan.Id, Arg.Any<CancellationToken>()).Returns(ratePlan);
        _roomRepository.GetById(room.Id, Arg.Any<CancellationToken>()).Returns(room);
        _fiscalAccountRepository.GetByOriginatorId(reservation.Id, Arg.Any<CancellationToken>()).Returns(fiscalAccount);
        _businessDateProvider.GetCurrentBusinessDate(Arg.Any<CancellationToken>()).Returns(new DateOnly(2026, 8, 11));

        var command = new PostRoomChargeCommand(reservation.Id);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBe(Guid.Empty);

        var mainFolio = fiscalAccount.Folios.Single();
        var item = mainFolio.Items.Single();
        item.Description.Should().Be("Room charge");
        item.Amount.Should().Be(150m);
        item.Quantity.Should().Be(1);
        item.TransactionCodeId.Should().Be(ratePlan.TransactionCodeId);
        item.TransactionType.Should().Be(FolioItemType.Charge);
    }

    [Fact]
    public async Task Handle_WhenRoomNotInRatePlan_ShouldThrowRatePlanInvalidForRoomException()
    {
        // Arrange
        var ratePlan = RatePlan.Create(
            "Standard",
            Guid.NewGuid(),
            new DateOnly(2026, 1, 1),
            new DateOnly(2026, 12, 31),
            [new RoomTypePriceDefinition(Guid.NewGuid(), 150m)]);

        var room = Room.Create("101", Guid.NewGuid());

        var reservation = await Reservation.Create(
            Guid.NewGuid(),
            room.Id,
            ratePlan.Id,
            "RES-1",
            new DateOnly(2026, 8, 10),
            new DateOnly(2026, 8, 12),
            null,
            [Guid.NewGuid()],
            _roomAvailabilityService);

        _reservationRepository.GetById(reservation.Id, Arg.Any<CancellationToken>()).Returns(reservation);
        _ratePlanRepository.GetById(ratePlan.Id, Arg.Any<CancellationToken>()).Returns(ratePlan);
        _roomRepository.GetById(room.Id, Arg.Any<CancellationToken>()).Returns(room);

        var command = new PostRoomChargeCommand(reservation.Id);

        // Act
        Func<Task> act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<RatePlanInvalidForRoomException>();
    }
}