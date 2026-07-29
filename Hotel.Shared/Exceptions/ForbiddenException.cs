namespace Hotel.Shared.Exceptions;

public class ForbiddenException(string message = "Forbidden") : DomainException(message)
{
}