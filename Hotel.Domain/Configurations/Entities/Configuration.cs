namespace Hotel.Domain.Configurations.Entities;

public class Configuration
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public TimeZoneInfo TimeZone { get; set; } = TimeZoneInfo.Utc;

    public DateOnly CurrentBusinessDate { get; set; }

    public static Configuration Create(string timeZoneId, DateOnly currentBusinessDate)
    {
        var timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);

        return new Configuration
        {
            Id = Guid.NewGuid(),
            TimeZone = timeZone,
            CurrentBusinessDate = currentBusinessDate,
        };
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