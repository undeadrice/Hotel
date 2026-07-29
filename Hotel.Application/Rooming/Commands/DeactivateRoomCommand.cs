using Hotel.Application.Pipeline;

namespace Hotel.Application.Rooming.Commands;

public record DeactivateRoomCommand(Guid RoomId) : ICommand;