using Hotel.Application.Configurations.TransferObjects;
using MediatR;
using Hotel.Domain.Configurations.Repositories;
using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;

namespace Hotel.Application.Configurations.Queries;

[CheckPermission(Permission.ConfigurationView)]
public record GetConfigurationQuery : IRequest<ConfigurationDto?>;

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
