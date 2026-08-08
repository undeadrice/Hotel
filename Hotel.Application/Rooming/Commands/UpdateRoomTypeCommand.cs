using Hotel.Application.Pipeline;

namespace Hotel.Application.Rooming.Commands;

public record UpdateRoomTypeCommand(
    Guid Id,
    string Name,
    string? Description)
    : ICommand;
