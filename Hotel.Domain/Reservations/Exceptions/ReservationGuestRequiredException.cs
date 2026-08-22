using Hotel.Shared.Exceptions;

namespace Hotel.Domain.Reservations.Exceptions;

public class ReservationGuestRequiredException() : DomainException("At least one guest must be assigned.")
{
}