using Hotel.Domain.Configurations.Repositories;
using MediatR;

namespace Hotel.Application.Configurations.Queries;

public record GetConfigurationSeedStatusQuery : IRequest<bool>;

internal class GetConfigurationSeedStatusQueryHandler(IConfigurationRepository configurationRepository)
    : IRequestHandler<GetConfigurationSeedStatusQuery, bool>
{
    public async Task<bool> Handle(GetConfigurationSeedStatusQuery request, CancellationToken cancellationToken)
    {
        var configuration = await configurationRepository.Get(cancellationToken);

        return configuration.IsSeeded;
    }
}