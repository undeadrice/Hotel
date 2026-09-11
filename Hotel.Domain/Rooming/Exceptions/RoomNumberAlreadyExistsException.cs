using Hotel.Shared.Exceptions;

namespace Hotel.Domain.Rooming.Exceptions;

public class RoomNumberAlreadyExistsException(string roomNumber)
    : DomainException($"Room with number '{roomNumber}' already exists.")
{
}