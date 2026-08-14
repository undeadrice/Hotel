using Hotel.Application.Configurations.TransferObjects;
using Hotel.Domain.Configurations.Services;
using MediatR;

namespace Hotel.Application.Configurations.Queries;

internal class GetConfigurationQueryHandler(IConfigurationRepository configurationRepository)
    : IRequestHandler<GetConfigurationQuery, ConfigurationDto?>
{
    public async Task<ConfigurationDto?> Handle(GetConfigurationQuery request, CancellationToken cancellationToken)
    {
        var configuration = await configurationRepository.Find(cancellationToken);

        if (configuration is null)
        {
            return null;
        }

        return new ConfigurationDto(
            configuration.Id,
            configuration.TimeZone.Id,
            configuration.CurrentBusinessDate);
    }
}