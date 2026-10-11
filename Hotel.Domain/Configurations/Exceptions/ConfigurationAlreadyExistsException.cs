using Hotel.Shared.Exceptions;

namespace Hotel.Domain.Configurations.Exceptions;

public class ConfigurationAlreadyExistsException()
    : DomainException("Configuration already exists.")
{
}