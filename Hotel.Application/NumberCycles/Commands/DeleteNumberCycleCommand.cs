using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;

namespace Hotel.Application.NumberCycles.Commands;

[CheckPermission(Permission.NumberCycleDelete)]
public record DeleteNumberCycleCommand(Guid Id) : ICommand;