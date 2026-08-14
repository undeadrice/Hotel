using Hotel.Domain.Configurations.Entities;
using Hotel.Domain.Configurations.Services;
using MediatR;

namespace Hotel.Application.Configurations.Commands;

public class UpsertConfigurationCommandHandler(IConfigurationRepository configurationRepository)
    : IRequestHandler<UpsertConfigurationCommand, Guid>
{
    public async Task<Guid> Handle(UpsertConfigurationCommand request, CancellationToken cancellationToken)
    {
        var existing = await configurationRepository.Find(cancellationToken);

        if (existing is null)
        {
            var configuration = Configuration.Create(request.TimeZoneId, request.CurrentBusinessDate);
            await configurationRepository.Add(configuration, cancellationToken);
            return configuration.Id;
        }

        existing.Update(request.TimeZoneId, request.CurrentBusinessDate);
        return existing.Id;
    }
}