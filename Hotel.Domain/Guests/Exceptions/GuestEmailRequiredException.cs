using Hotel.Shared.Exceptions;

namespace Hotel.Domain.Guests.Exceptions;

public class GuestEmailRequiredException() : DomainException("Guest email is required.")
{
}