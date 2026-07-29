using Hotel.Application.Pipeline;

namespace Hotel.Application.Products.Commands;

public record SeedProductsCommand(int Quantity) : ICommand<int>;
