using Hotel.Application.Pipeline;

namespace Hotel.Application.Rooming.Commands;

public record UpdateRoomCommand(
    Guid Id,
    string RoomNumber,
    Guid RoomTypeId)
    : ICommand;