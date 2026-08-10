using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;

namespace Hotel.Application.Rooming.Commands;

[CheckPermission(Permission.RoomTypeEdit)]
public record UpdateRoomTypeCommand(
    Guid Id,
    string Name,
    string? Description)
    : ICommand;
