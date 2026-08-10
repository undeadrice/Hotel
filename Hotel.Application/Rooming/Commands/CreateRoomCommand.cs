using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;

namespace Hotel.Application.Rooming.Commands;

[CheckPermission(Permission.RoomCreate)]
public record CreateRoomCommand(
    string RoomNumber,
    Guid RoomTypeId)
    : ICommand<Guid>;
