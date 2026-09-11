using Hotel.Shared.Exceptions;

namespace Hotel.Domain.Reservations.Exceptions;

public class ReservationInvalidDateRangeException() : DomainException("Start date must be before end date.")
{
}