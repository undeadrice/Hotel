using Hotel.Shared.Exceptions;

namespace Hotel.Domain.Guests.Exceptions;

public class GuestLastNameRequiredException() : DomainException("Guest last name is required.")
{
}