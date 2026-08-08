namespace Hotel.Domain.Reservations.Entities;

public class ReservationGuest
{
    public Guid ReservationId { get; set; }
    public Guid GuestId { get; set; }

#pragma warning disable CS8618
    public ReservationGuest() { }
#pragma warning restore CS8618
}