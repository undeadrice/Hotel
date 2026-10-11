using Hotel.Shared.Exceptions;

namespace Hotel.Domain.Reservations.Exceptions;

public class ReservationNotInHouseException() : DomainException("Reservation is not in InHouse status, so check-out cannot be performed.")
{
}