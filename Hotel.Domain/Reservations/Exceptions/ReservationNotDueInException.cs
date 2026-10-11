using Hotel.Shared.Exceptions;

namespace Hotel.Domain.Reservations.Exceptions;

public class ReservationNotDueInException() : DomainException("Reservation is not in DueIn status, so check-in cannot be performed.")
{
}