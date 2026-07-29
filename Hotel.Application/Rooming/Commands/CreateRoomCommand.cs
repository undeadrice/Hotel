using Hotel.Application.Pipeline;

namespace Hotel.Application.Rooming.Commands;

public record CreateRoomCommand(
    string RoomNumber,
    Guid RoomTypeId)
    : ICommand<Guid>;