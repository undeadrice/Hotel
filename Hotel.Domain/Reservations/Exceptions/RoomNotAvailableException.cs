using Hotel.Shared.Exceptions;

namespace Hotel.Domain.Reservations.Exceptions;

public class RoomNotAvailableException() : DomainException("The selected room is not available for the given date range.")
{
}