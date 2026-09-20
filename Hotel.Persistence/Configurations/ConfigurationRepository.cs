using Hotel.Domain.Configurations.Entities;
using Hotel.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;
using Hotel.Domain.Configurations.Repositories;

namespace Hotel.Persistence.Configurations;

public class ConfigurationRepository(PersistenceDbContext dbContext) : IConfigurationRepository
{
    public async Task<Configuration?> Find(CancellationToken token = default)
    {
        return await dbContext.Configurations.FirstOrDefaultAsync(token);
    }

    public async Task<Configuration> Get(CancellationToken token = default)
    {
        var configuration = await Find(token);

        if (configuration is null)
        {
            throw new NotFoundException("Configuration doesn't exist");
        }

        return configuration;
    }

    public async Task Add(Configuration configuration, CancellationToken token = default)
    {
        await dbContext.Configurations.AddAsync(configuration, token);
    }
}