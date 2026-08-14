using Hotel.Domain.Configurations.Entities;

namespace Hotel.Domain.Configurations.Services;

public interface IConfigurationRepository
{
    Task<Configuration?> Find(CancellationToken token = default);

    Task<Configuration> Get(CancellationToken token = default);

    Task Add(Configuration configuration, CancellationToken token = default);
}