using Hotel.Domain.Configurations.Entities;
using MediatR;
using Hotel.Domain.Configurations.Repositories;
using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;

namespace Hotel.Application.Configurations.Commands;

[CheckPermission(Permission.ConfigurationEdit)]
public record UpsertConfigurationCommand(
    string TimeZoneId,
    DateOnly CurrentBusinessDate)
    : ICommand<Guid>;

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
