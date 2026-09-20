using Hotel.Shared.Exceptions;

namespace Hotel.Domain.Rooming.Exceptions;

public class RoomNumberRequiredException() : DomainException("Room number is required.")
{
}