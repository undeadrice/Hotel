using Hotel.Domain.Reservations.Enums;
using Hotel.Domain.Reservations.Exceptions;
using Hotel.Domain.Reservations.Services;

namespace Hotel.Domain.Reservations.Entities;

public class Reservation
{
    public Guid Id { get; private set; }
    public Guid CreatorId { get; private set; }
    public Guid RoomId { get; private set; }
    public Guid RatePlanId { get; private set; }
    public string CycleIdentifier { get; private set; }
    public DateOnly StartDate { get; private set; }
    public DateOnly EndDate { get; private set; }
    public DateTime? ArrivalTime { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public ReservationStatus Status { get; private set; }

    private readonly List<ReservationGuest> _guests = new();
    public IReadOnlyCollection<ReservationGuest> Guests => _guests.AsReadOnly();

#pragma warning disable CS8618
    public Reservation() { }
#pragma warning restore CS8618

    private Reservation(
        Guid id,
        Guid creatorId,
        Guid roomId,
        Guid ratePlanId,
        string cycleIdentifier,
        DateOnly startDate,
        DateOnly endDate,
        DateTime? arrivalTime,
        DateTime createdAt)
    {
        Id = id;
        CreatorId = creatorId;
        RoomId = roomId;
        RatePlanId = ratePlanId;
        CycleIdentifier = cycleIdentifier;
        StartDate = startDate;
        EndDate = endDate;
        ArrivalTime = arrivalTime;
        CreatedAt = createdAt;
        Status = ReservationStatus.Reserved;
    }

    public static async Task<Reservation> Create(
        Guid creatorId,
        Guid roomId,
        Guid ratePlanId,
        string cycleIdentifier,
        DateOnly startDate,
        DateOnly endDate,
        DateTime? arrivalTime,
        IEnumerable<Guid> guestIds,
        IRoomAvailabilityService roomAvailabilityService,
        CancellationToken cancellationToken = default)
    {
        if (startDate >= endDate)
        {
            throw new ArgumentException("Start date must be before end date.");
        }

        if (!guestIds.Any())
        {
            throw new ArgumentException("At least one guest must be assigned.");
        }

        if (string.IsNullOrWhiteSpace(cycleIdentifier))
        {
            throw new ArgumentException("Cycle identifier is required.", nameof(cycleIdentifier));
        }

        var isOccupied = await roomAvailabilityService.IsRoomOccupied(roomId, startDate, endDate, cancellationToken);

        if (isOccupied)
        {
            throw new RoomNotAvailableException();
        }

        var reservation = new Reservation(
            Guid.NewGuid(),
            creatorId,
            roomId,
            ratePlanId,
            cycleIdentifier,
            startDate,
            endDate,
            arrivalTime,
            DateTime.UtcNow);

        foreach (var guestId in guestIds)
        {
            reservation.AddGuest(guestId);
        }

        return reservation;
    }

    public void CheckIn()
    {
        if (Status != ReservationStatus.DueIn)
        {
            throw new ReservationNotDueInException();
        }

        Status = ReservationStatus.InHouse;
    }

    public void TransitionOnEndOfDay(DateOnly businessDate)
    {
        if (Status == ReservationStatus.Reserved && StartDate == businessDate)
        {
            Status = ReservationStatus.DueIn;
            return;
        }

        if (Status == ReservationStatus.DueIn)
        {
            Status = ReservationStatus.NoShow;
        }
    }

    private void AddGuest(Guid guestId)
    {
        if (!_guests.Any(g => g.GuestId == guestId))
        {
            _guests.Add(new ReservationGuest { ReservationId = Id, GuestId = guestId });
        }
    }
}