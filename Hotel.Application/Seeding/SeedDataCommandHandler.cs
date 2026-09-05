using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;
using MediatR;

namespace Hotel.Application.Seeding;

[CheckRole([UserRole.SuperAdmin])]
public record SeedDataCommand(
    string TimeZoneId,
    DateOnly CurrentBusinessDate,
    bool SeedBusinessData) : ICommand<Guid>;

public class SeedDataCommandHandler(ISeedDataService seedDataService)
    : IRequestHandler<SeedDataCommand, Guid>
{
    public async Task<Guid> Handle(SeedDataCommand request, CancellationToken cancellationToken)
    {
        return await seedDataService.SeedAsync(
            request.TimeZoneId,
            request.CurrentBusinessDate,
            request.SeedBusinessData,
            cancellationToken);
    }
}