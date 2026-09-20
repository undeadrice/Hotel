using Hotel.Shared.Exceptions;

namespace Hotel.Domain.Reservations.Exceptions;

public class RoomNotActiveException() : DomainException("The selected room is not active.")
{
}