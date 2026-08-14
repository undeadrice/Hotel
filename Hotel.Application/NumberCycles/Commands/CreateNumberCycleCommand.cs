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