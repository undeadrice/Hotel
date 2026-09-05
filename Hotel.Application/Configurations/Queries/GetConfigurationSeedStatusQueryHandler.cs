using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;
using Hotel.Domain.Configurations.Repositories;
using MediatR;

namespace Hotel.Application.Configurations.Queries;

[CheckPermission(Permission.ConfigurationView)]
public record GetConfigurationSeedStatusQuery : IRequest<bool>;

internal class GetConfigurationSeedStatusQueryHandler(IConfigurationRepository configurationRepository)
    : IRequestHandler<GetConfigurationSeedStatusQuery, bool>
{
    public async Task<bool> Handle(GetConfigurationSeedStatusQuery request, CancellationToken cancellationToken)
    {
        var configuration = await configurationRepository.Find(cancellationToken);

        return configuration?.IsSeeded ?? false;
    }
}