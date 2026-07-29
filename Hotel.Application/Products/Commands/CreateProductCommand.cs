using Hotel.Application.Pipeline;

namespace Hotel.Application.Products.Commands;

public record CreateProductCommand(
    string Name,
    string Description,
    decimal Price,
    int Stock)
    : ICommand<Guid>;

