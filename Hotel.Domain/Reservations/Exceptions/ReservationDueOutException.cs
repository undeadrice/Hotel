using Hotel.Shared.Exceptions;

namespace Hotel.Domain.Reservations.Exceptions;

public class ReservationDueOutException()
    : DomainException("The last night charge must be posted manually. Any fiscal accounts connected to reservations that are about to be finished (last day) must be paid and checked out.")
{
}