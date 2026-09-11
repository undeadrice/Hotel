namespace Hotel.IntegrationTests.Infrastructure.TestData;

/// <summary>
/// Provides dates relative to the business date seeded by the test factory.
/// </summary>
public static class RatePlanDates
{
    public static DateOnly BusinessDate => DateOnly.FromDateTime(DateTime.UtcNow.AddYears(1));

    public static DateOnly ValidStartDate => BusinessDate.AddDays(1);

    public static DateOnly ValidEndDate => BusinessDate.AddYears(1);
}