using Hotel.Application.Pipeline;

namespace Hotel.Application.Rooming.Commands;

public record CreateRoomTypeCommand(
    string Name,
    decimal BaseRate,
    string? Description)
    : ICommand<Guid>;