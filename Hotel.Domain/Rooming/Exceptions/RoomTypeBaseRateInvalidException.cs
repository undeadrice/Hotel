using Hotel.Shared.Exceptions;

namespace Hotel.Domain.Rooming.Exceptions;

public class RoomTypeBaseRateInvalidException() : DomainException("Room type base rate must be greater than zero.")
{
}