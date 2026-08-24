using FluentAssertions;
using Hotel.Application.Reservations.Commands;
using Hotel.Domain.FiscalAccounting.Entities;
using Hotel.Domain.FiscalAccounting.Repositories;
using Hotel.Domain.NumberCycles.Enums;
using Hotel.Domain.NumberCycles.Services;
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

namespace Hotel.Application.Tests.Reservations.Commands;

public class CreateReservationCommandHandlerTests
{
    private const string ReservationIdentifier = "RES-1";
    private const string FiscalAccountIdentifier = "FA-1";

    private readonly IReservationRepository _reservationRepository;
    private readonly IFiscalAccountRepository _fiscalAccountRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IRatePlanRepository _ratePlanRepository;
    private readonly IRoomAvailabilityService _roomAvailabilityService;
    private readonly INumberCycleService _numberCycleService;
    private readonly CreateReservationCommandHandler _handler;

    private readonly Guid _creatorId = Guid.NewGuid();
    private readonly Guid _roomTypeId = Guid.NewGuid();
    private readonly Room _room;
    private readonly RatePlan _ratePlan;
    private readonly DateOnly _startDate = new(2026, 8, 10);
    private readonly DateOnly _endDate = new(2026, 8, 12);

    public CreateReservationCommandHandlerTests()
    {
        _reservationRepository = Substitute.For<IReservationRepository>();
        _fiscalAccountRepository = Substitute.For<IFiscalAccountRepository>();
        _roomRepository = Substitute.For<IRoomRepository>();
        _ratePlanRepository = Substitute.For<IRatePlanRepository>();
        _roomAvailabilityService = Substitute.For<IRoomAvailabilityService>();
        _numberCycleService = Substitute.For<INumberCycleService>();

        _handler = new CreateReservationCommandHandler(
            _reservationRepository,
            _fiscalAccountRepository,
            _roomRepository,
            _ratePlanRepository,
            _roomAvailabilityService,
            _numberCycleService);

        _room = Room.Create("101", _roomTypeId);
        _ratePlan = RatePlan.Create(
            "Standard",
            Guid.NewGuid(),
            new DateOnly(2026, 8, 1),
            new DateOnly(2026, 8, 31),
            [new RoomTypePriceDefinition(_roomTypeId, 100m)]);

        _roomRepository.GetById(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(_room);
        _ratePlanRepository.GetById(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(_ratePlan);

        _roomAvailabilityService
            .IsRoomOccupied(Arg.Any<Guid>(), Arg.Any<DateOnly>(), Arg.Any<DateOnly>(), Arg.Any<CancellationToken>())
            .Returns(false);

        _numberCycleService
            .NextIdentifier(NumberCycleTopic.Reservation, Arg.Any<CancellationToken>())
            .Returns(ReservationIdentifier);

        _numberCycleService
            .NextIdentifier(NumberCycleTopic.FiscalAccount, Arg.Any<CancellationToken>())
            .Returns(FiscalAccountIdentifier);
    }

    private CreateReservationCommand CreateCommand() =>
        new(
            _creatorId,
            _room.Id,
            _ratePlan.Id,
            _startDate,
            _endDate,
            null,
            [Guid.NewGuid()]);

    [Fact]
    public async Task Handle_WithValidCommand_ShouldAddReservationAndFiscalAccountAndReturnId()
    {
        // Arrange
        var command = CreateCommand();

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBe(Guid.Empty);

        await _reservationRepository.Received(1).Add(Arg.Any<Reservation>(), Arg.Any<CancellationToken>());
        await _fiscalAccountRepository.Received(1).Add(Arg.Any<FiscalAccount>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldCreateFiscalAccountLinkedToReservation()
    {
        // Arrange
        var command = CreateCommand();
        FiscalAccount? addedAccount = null;

        await _fiscalAccountRepository.Add(
            Arg.Do<FiscalAccount>(account => addedAccount = account),
            Arg.Any<CancellationToken>());

        // Act
        var reservationId = await _handler.Handle(command, CancellationToken.None);

        // Assert
        addedAccount.Should().NotBeNull();
        addedAccount.OriginatorId.Should().Be(reservationId);
        addedAccount.OwnerId.Should().Be(_creatorId);
    }

    [Fact]
    public async Task Handle_ShouldPassServiceIdentifiersToReservationAndFiscalAccount()
    {
        // Arrange
        var command = CreateCommand();
        Reservation? addedReservation = null;
        FiscalAccount? addedAccount = null;

        await _reservationRepository.Add(
            Arg.Do<Reservation>(reservation => addedReservation = reservation),
            Arg.Any<CancellationToken>());

        await _fiscalAccountRepository.Add(
            Arg.Do<FiscalAccount>(account => addedAccount = account),
            Arg.Any<CancellationToken>());

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        addedReservation.Should().NotBeNull();
        addedReservation.CycleIdentifier.Should().Be(ReservationIdentifier);

        addedAccount.Should().NotBeNull();
        addedAccount.CycleIdentifier.Should().Be(FiscalAccountIdentifier);

        await _numberCycleService.Received(1).NextIdentifier(NumberCycleTopic.Reservation, Arg.Any<CancellationToken>());
        await _numberCycleService.Received(1).NextIdentifier(NumberCycleTopic.FiscalAccount, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenRoomIsNotActive_ShouldThrowRoomNotActiveException()
    {
        // Arrange
        var inactiveRoom = Room.Create("102", _roomTypeId);
        inactiveRoom.Deactivate();

        _roomRepository.GetById(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(inactiveRoom);

        var command = CreateCommand() with { RoomId = inactiveRoom.Id };

        // Act
        Func<Task> act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<RoomNotActiveException>();
    }

    [Fact]
    public async Task Handle_WhenReservationDatesOutsideRatePlan_ShouldThrowRatePlanInvalidForRoomException()
    {
        // Arrange
        var command = CreateCommand() with
        {
            StartDate = new DateOnly(2026, 7, 1),
            EndDate = new DateOnly(2026, 7, 5)
        };

        // Act
        Func<Task> act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<RatePlanInvalidForRoomException>();
    }

    [Fact]
    public async Task Handle_WhenRoomNotInRatePlan_ShouldThrowRatePlanInvalidForRoomException()
    {
        // Arrange
        var room = Room.Create("103", Guid.NewGuid());

        _roomRepository.GetById(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(room);

        var command = CreateCommand() with { RoomId = room.Id };

        // Act
        Func<Task> act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<RatePlanInvalidForRoomException>();
    }
}