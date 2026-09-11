using Hotel.Shared.Exceptions;

namespace Hotel.Domain.Rooming.Exceptions;

public class RoomTypeNameRequiredException() : DomainException("Room type name is required.")
{
}