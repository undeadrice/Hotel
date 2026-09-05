namespace Hotel.Domain.Configurations.Entities;

public class Configuration
{
    public Guid Id { get; private set; }

    public TimeZoneInfo TimeZone { get; private set; }

    public DateOnly CurrentBusinessDate { get; private set; }

    public bool IsSeeded { get; private set; }

#pragma warning disable CS8618
    public Configuration() { }
#pragma warning restore CS8618

    private Configuration(Guid id, TimeZoneInfo timeZone, DateOnly currentBusinessDate, bool isSeeded)
    {
        Id = id;
        TimeZone = timeZone;
        CurrentBusinessDate = currentBusinessDate;
        IsSeeded = isSeeded;
    }

    public static Configuration Create(string timeZoneId, DateOnly currentBusinessDate, bool isSeeded = false)
    {
        var timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);

        return new Configuration(Guid.NewGuid(), timeZone, currentBusinessDate, isSeeded);
    }

    public void MarkSeeded()
    {
        IsSeeded = true;
    }

    public void EndOfDay()
    {
        CurrentBusinessDate = CurrentBusinessDate.AddDays(1);
    }
}