using Hotel.Application.Configurations.TransferObjects;
using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;
using MediatR;

namespace Hotel.Application.Configurations.Queries;

[CheckPermission(Permission.ConfigurationView)]
public record GetServerTimeZonesQuery()
    : IRequest<IReadOnlyCollection<TimeZoneDto>>;