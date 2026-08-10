using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;

namespace Hotel.Application.Rooming.Commands;

[CheckPermission(Permission.RoomEdit)]
public record UpdateRoomCommand(
    Guid Id,
    string RoomNumber,
    Guid RoomTypeId)
    : ICommand;
