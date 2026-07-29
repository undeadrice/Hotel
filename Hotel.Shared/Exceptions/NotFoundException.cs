namespace Hotel.Shared.Exceptions;

public class NotFoundException(string message) : DomainException(message)
{
}
