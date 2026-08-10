using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;

namespace Hotel.Application.Rooming.Commands;

[CheckPermission(Permission.RoomTypeCreate)]
public record CreateRoomTypeCommand(
    string Name,
    string? Description)
    : ICommand<Guid>;
