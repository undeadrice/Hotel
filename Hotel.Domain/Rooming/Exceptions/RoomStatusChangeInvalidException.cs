using Hotel.Shared.Exceptions;

namespace Hotel.Domain.Rooming.Exceptions;

public class RoomStatusChangeInvalidException(string message) : DomainException(message)
{
}