using Hotel.Application.Pipeline;

namespace Hotel.Application.Rooming.Commands;

public record CreateRoomTypeCommand(
    string Name,
    string? Description)
    : ICommand<Guid>;
