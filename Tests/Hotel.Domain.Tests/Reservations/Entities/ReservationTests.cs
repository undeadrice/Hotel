using FluentAssertions;
using Hotel.Domain.Reservations.Entities;
using Hotel.Domain.Reservations.Enums;
using Hotel.Domain.Reservations.Exceptions;
using Hotel.Domain.Reservations.Services;
using NSubstitute;
using Xunit;

namespace Hotel.Domain.Tests.Reservations.Entities;

public class ReservationTests
{
    private static readonly Guid CreatorId = Guid.NewGuid();
    private static readonly Guid RoomId = Guid.NewGuid();
    private static readonly Guid RatePlanId = Guid.NewGuid();
    private const string CycleIdentifier = "CY-2026";
    private static readonly DateOnly StartDate = new(2026, 8, 10);
    private static readonly DateOnly EndDate = new(2026, 8, 12);
    private static readonly Guid[] GuestIds = [Guid.NewGuid(), Guid.NewGuid()];

    private readonly IRoomAvailabilityService _roomAvailabilityService;

    public ReservationTests()
    {
        _roomAvailabilityService = Substitute.For<IRoomAvailabilityService>();

        _roomAvailabilityService
            .IsRoomOccupied(Arg.Any<Guid>(), Arg.Any<DateOnly>(), Arg.Any<DateOnly>(), Arg.Any<CancellationToken>())
            .Returns(false);
    }

    private Task<Reservation> CreateReservation(
        DateOnly? startDate = null,
        DateOnly? endDate = null,
        IEnumerable<Guid>? guestIds = null,
        string? cycleIdentifier = CycleIdentifier)
    {
        return Reservation.Create(
            CreatorId,
            RoomId,
            RatePlanId,
            cycleIdentifier!,
            startDate ?? StartDate,
            endDate ?? EndDate,
            null,
            guestIds ?? GuestIds,
            _roomAvailabilityService);
    }

    [Fact]
    public async Task Create_WithValidArguments_ShouldCreateReservedReservation()
    {
        // Act
        var reservation = await CreateReservation();

        // Assert
        reservation.Id.Should().NotBe(Guid.Empty);
        reservation.CreatorId.Should().Be(CreatorId);
        reservation.RoomId.Should().Be(RoomId);
        reservation.RatePlanId.Should().Be(RatePlanId);
        reservation.CycleIdentifier.Should().Be(CycleIdentifier);
        reservation.StartDate.Should().Be(StartDate);
        reservation.EndDate.Should().Be(EndDate);
        reservation.Status.Should().Be(ReservationStatus.Reserved);
        reservation.Guests.Should().HaveCount(2);
        reservation.Guests.Select(g => g.GuestId).Should().BeEquivalentTo(GuestIds);
    }

    [Fact]
    public async Task Create_WithDuplicateGuestIds_ShouldAddGuestOnlyOnce()
    {
        // Arrange
        var duplicateId = Guid.NewGuid();

        // Act
        var reservation = await CreateReservation(guestIds: [duplicateId, duplicateId]);

        // Assert
        reservation.Guests.Should().ContainSingle(g => g.GuestId == duplicateId);
    }

    [Fact]
    public async Task Create_WithStartDateNotBeforeEndDate_ShouldThrowReservationInvalidDateRangeException()
    {
        // Act
        Func<Task> act = () => CreateReservation(startDate: EndDate, endDate: StartDate);

        // Assert
        await act.Should().ThrowAsync<ReservationInvalidDateRangeException>();
    }

    [Fact]
    public async Task Create_WithEmptyGuestList_ShouldThrowReservationGuestRequiredException()
    {
        // Act
        Func<Task> act = () => CreateReservation(guestIds: []);

        // Assert
        await act.Should().ThrowAsync<ReservationGuestRequiredException>();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task Create_WithInvalidCycleIdentifier_ShouldThrowReservationCycleIdentifierRequiredException(string? cycleIdentifier)
    {
        // Act
        Func<Task> act = () => CreateReservation(cycleIdentifier: cycleIdentifier);

        // Assert
        await act.Should().ThrowAsync<ReservationCycleIdentifierRequiredException>();
    }

    [Fact]
    public async Task Create_WhenRoomIsOccupied_ShouldThrowRoomNotAvailableException()
    {
        // Arrange
        _roomAvailabilityService
            .IsRoomOccupied(RoomId, StartDate, EndDate, Arg.Any<CancellationToken>())
            .Returns(true);

        // Act
        Func<Task> act = () => CreateReservation();

        // Assert
        await act.Should().ThrowAsync<RoomNotAvailableException>();
    }

    [Fact]
    public async Task CheckIn_WhenDueIn_ShouldSetInHouse()
    {
        // Arrange
        var reservation = await CreateReservation();
        reservation.TransitionOnEndOfDay(StartDate);

        // Act
        reservation.CheckIn();

        // Assert
        reservation.Status.Should().Be(ReservationStatus.InHouse);
    }

    [Fact]
    public async Task CheckIn_WhenReserved_ShouldThrowReservationNotDueInException()
    {
        // Arrange
        var reservation = await CreateReservation();

        // Act
        Action act = () => reservation.CheckIn();

        // Assert
        act.Should().Throw<ReservationNotDueInException>();
    }

    [Fact]
    public async Task CheckOut_WhenInHouse_ShouldSetCheckedOut()
    {
        // Arrange
        var reservation = await CreateReservation();
        reservation.TransitionOnEndOfDay(StartDate);
        reservation.CheckIn();

        // Act
        reservation.CheckOut();

        // Assert
        reservation.Status.Should().Be(ReservationStatus.CheckedOut);
    }

    [Fact]
    public async Task CheckOut_WhenReserved_ShouldThrowReservationNotInHouseException()
    {
        // Arrange
        var reservation = await CreateReservation();

        // Act
        Action act = () => reservation.CheckOut();

        // Assert
        act.Should().Throw<ReservationNotInHouseException>();
    }

    [Fact]
    public async Task TransitionOnEndOfDay_WhenReservedAndStartDateIsBusinessDate_ShouldSetDueIn()
    {
        // Arrange
        var reservation = await CreateReservation();

        // Act
        reservation.TransitionOnEndOfDay(StartDate);

        // Assert
        reservation.Status.Should().Be(ReservationStatus.DueIn);
    }

    [Fact]
    public async Task TransitionOnEndOfDay_WhenReservedAndStartDateIsNotBusinessDate_ShouldRemainReserved()
    {
        // Arrange
        var reservation = await CreateReservation();

        // Act
        reservation.TransitionOnEndOfDay(StartDate.AddDays(-1));

        // Assert
        reservation.Status.Should().Be(ReservationStatus.Reserved);
    }

    [Fact]
    public async Task TransitionOnEndOfDay_WhenDueIn_ShouldSetNoShow()
    {
        // Arrange
        var reservation = await CreateReservation();
        reservation.TransitionOnEndOfDay(StartDate);

        // Act
        reservation.TransitionOnEndOfDay(StartDate.AddDays(1));

        // Assert
        reservation.Status.Should().Be(ReservationStatus.NoShow);
    }

    [Fact]
    public async Task TransitionOnEndOfDay_WhenInHouse_ShouldRemainInHouse()
    {
        // Arrange
        var reservation = await CreateReservation();
        reservation.TransitionOnEndOfDay(StartDate);
        reservation.CheckIn();

        // Act
        reservation.TransitionOnEndOfDay(StartDate.AddDays(1));

        // Assert
        reservation.Status.Should().Be(ReservationStatus.InHouse);
    }
}