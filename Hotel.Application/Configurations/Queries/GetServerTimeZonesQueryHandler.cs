using Hotel.Application.Configurations.TransferObjects;
using MediatR;
using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;

namespace Hotel.Application.Configurations.Queries;

[CheckPermission(Permission.ConfigurationView)]
public record GetServerTimeZonesQuery()
    : IRequest<IReadOnlyCollection<TimeZoneDto>>;

internal class GetServerTimeZonesQueryHandler
    : IRequestHandler<GetServerTimeZonesQuery, IReadOnlyCollection<TimeZoneDto>>
{
    public Task<IReadOnlyCollection<TimeZoneDto>> Handle(
        GetServerTimeZonesQuery request,
        CancellationToken cancellationToken)
    {
        var timeZones = TimeZoneInfo.GetSystemTimeZones()
            .Select(tz => new TimeZoneDto(tz.Id, tz.DisplayName))
            .OrderBy(tz => tz.DisplayName, StringComparer.OrdinalIgnoreCase)
            .ToArray();

        return Task.FromResult<IReadOnlyCollection<TimeZoneDto>>(timeZones);
    }
}
