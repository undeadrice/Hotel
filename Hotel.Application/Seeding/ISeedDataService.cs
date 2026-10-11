namespace Hotel.Application.Seeding;

public interface ISeedDataService
{
    Task<Guid> SeedAsync(
        string timeZoneId,
        DateOnly currentBusinessDate,
        bool seedBusinessData,
        CancellationToken cancellationToken = default);
}