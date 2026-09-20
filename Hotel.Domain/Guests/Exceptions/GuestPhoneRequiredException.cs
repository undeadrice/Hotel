using Hotel.Shared.Exceptions;

namespace Hotel.Domain.Guests.Exceptions;

public class GuestPhoneRequiredException() : DomainException("Guest phone is required.")
{
}