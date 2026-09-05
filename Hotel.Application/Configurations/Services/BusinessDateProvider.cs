using Hotel.Domain.Configurations.Repositories;

namespace Hotel.Application.Configurations.Services;

public class BusinessDateProvider(IConfigurationRepository configurationRepository) : IBusinessDateProvider
{
    public async Task<DateOnly> GetCurrentBusinessDate(CancellationToken token = default)
    {
        var configuration = await configurationRepository.Get(token);
        return configuration.CurrentBusinessDate;
    }
}