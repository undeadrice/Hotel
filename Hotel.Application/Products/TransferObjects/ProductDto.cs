namespace Hotel.Application.Products.TransferObjects;

public record ProductDto(Guid Id, string Name, string Description, decimal Price, int Stock);