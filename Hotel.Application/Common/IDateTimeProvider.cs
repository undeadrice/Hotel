namespace Hotel.Application.Common;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}