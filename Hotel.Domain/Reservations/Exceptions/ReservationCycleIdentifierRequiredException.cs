using Hotel.Shared.Exceptions;

namespace Hotel.Domain.Reservations.Exceptions;

public class ReservationCycleIdentifierRequiredException() : DomainException("Cycle identifier is required.")
{
}