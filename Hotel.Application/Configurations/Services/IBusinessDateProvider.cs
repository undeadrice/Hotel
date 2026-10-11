namespace Hotel.Application.Configurations.Services;

public interface IBusinessDateProvider
{
    Task<DateOnly> GetCurrentBusinessDate(CancellationToken token = default);
}