namespace Hotel.Domain.Configurations.Entities;

public class Configuration
{
    public Guid Id { get; private set; }

    public TimeZoneInfo TimeZone { get; private set; }

    public DateOnly CurrentBusinessDate { get; private set; }

#pragma warning disable CS8618
    public Configuration() { }
#pragma warning restore CS8618

    private Configuration(Guid id, TimeZoneInfo timeZone, DateOnly currentBusinessDate)
    {
        Id = id;
        TimeZone = timeZone;
        CurrentBusinessDate = currentBusinessDate;
    }

    public static Configuration Create(string timeZoneId, DateOnly currentBusinessDate)
    {
        var timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);

        return new Configuration(Guid.NewGuid(), timeZone, currentBusinessDate);
    }

    public void Update(string timeZoneId, DateOnly currentBusinessDate)
    {
        TimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
        CurrentBusinessDate = currentBusinessDate;
    }

    public void EndOfDay()
    {
        CurrentBusinessDate = CurrentBusinessDate.AddDays(1);
    }
}