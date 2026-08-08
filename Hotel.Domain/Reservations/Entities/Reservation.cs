namespace Hotel.Domain.Reservations.Entities;

public class Reservation
{
    public Guid Id { get; private set; }
    public Guid CreatorId { get; private set; }
    public Guid RoomId { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private readonly List<ReservationGuest> _guests = new();
    public IReadOnlyCollection<ReservationGuest> Guests => _guests.AsReadOnly();

#pragma warning disable CS8618
    public Reservation() { }
#pragma warning restore CS8618

    private Reservation(
        Guid id,
        Guid creatorId,
        Guid roomId,
        DateTime startDate,
        DateTime endDate,
        DateTime createdAt)
    {
        Id = id;
        CreatorId = creatorId;
        RoomId = roomId;
        StartDate = startDate;
        EndDate = endDate;
        CreatedAt = createdAt;
    }

    public static Reservation Create(
        Guid creatorId,
        Guid roomId,
        DateTime startDate,
        DateTime endDate,
        IEnumerable<Guid> guestIds)
    {
        if (startDate >= endDate)
        {
            throw new ArgumentException("Start date must be before end date.");
        }

        if (!guestIds.Any())
        {
            throw new ArgumentException("At least one guest must be assigned.");
        }

        var reservation = new Reservation(
            Guid.NewGuid(),
            creatorId,
            roomId,
            startDate,
            endDate,
            DateTime.UtcNow);

        foreach (var guestId in guestIds)
        {
            reservation.AddGuest(guestId);
        }

        return reservation;
    }

    private void AddGuest(Guid guestId)
    {
        if (!_guests.Any(g => g.GuestId == guestId))
        {
            _guests.Add(new ReservationGuest { ReservationId = Id, GuestId = guestId });
        }
    }
}