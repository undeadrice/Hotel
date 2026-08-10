using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;

namespace Hotel.Application.Rooming.Commands;

[CheckPermission(Permission.RoomDelete)]
public record DeactivateRoomCommand(Guid RoomId) : ICommand;
