using Hotel.Shared.Exceptions;

namespace Hotel.Domain.Guests.Exceptions;

public class GuestDocumentNumberRequiredException() : DomainException("Guest document number is required.")
{
}