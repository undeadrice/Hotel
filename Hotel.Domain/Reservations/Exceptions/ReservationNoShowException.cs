using Hotel.Shared.Exceptions;

namespace Hotel.Domain.Reservations.Exceptions;

public class ReservationNoShowException()
    : DomainException("Reservation is in NoShow status, so check-out cannot be performed.")
{
}