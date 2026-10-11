using Hotel.Domain.NumberCycles.Services;
using MediatR;
using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;
using Hotel.Domain.NumberCycles.Enums;

namespace Hotel.Application.NumberCycles.Commands;

[CheckPermission(Permission.NumberCycleCreate)]
public record CreateNumberCycleCommand(
    NumberCycleTopic Topic,
    string Prefix,
    int StartIndex)
    : ICommand<Guid>;

internal class CreateNumberCycleCommandHandler(INumberCycleService numberCycleService)
    : IRequestHandler<CreateNumberCycleCommand, Guid>
{
    public async Task<Guid> Handle(CreateNumberCycleCommand request, CancellationToken cancellationToken)
    {
        var cycle = await numberCycleService.Create(
            request.Topic,
            request.Prefix,
            request.StartIndex,
            cancellationToken);

        return cycle.Id;
    }
}
