using Hotel.Shared.Exceptions;

namespace Hotel.Domain.Rooming.Exceptions;

public class RoomTypeNameAlreadyExistsException(string name)
    : DomainException($"Room type with name '{name}' already exists.")
{
}