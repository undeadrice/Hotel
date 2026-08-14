using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;

namespace Hotel.Application.Configurations.Commands;

[CheckPermission(Permission.ConfigurationEdit)]
public record UpsertConfigurationCommand(
    string TimeZoneId,
    DateOnly CurrentBusinessDate)
    : ICommand<Guid>;