using Hotel.Application.Pipeline;
using Hotel.Domain.Rooming.Enums;

namespace Hotel.Application.Rooming.Commands;

public record ChangeRoomStatusCommand(
    Guid RoomId,
    RoomStatus NewStatus)
    : ICommand;
