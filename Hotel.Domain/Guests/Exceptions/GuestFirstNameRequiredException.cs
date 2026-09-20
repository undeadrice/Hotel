using Hotel.Shared.Exceptions;

namespace Hotel.Domain.Guests.Exceptions;

public class GuestFirstNameRequiredException() : DomainException("Guest first name is required.")
{
}