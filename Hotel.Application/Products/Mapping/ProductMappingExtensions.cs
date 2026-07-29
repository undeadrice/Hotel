using Hotel.Application.Products.TransferObjects;
using Hotel.Domain.Products.Entities;

namespace Hotel.Application.Products.Mapping;

public static class ProductMappingExtensions
{
    public static ProductDto MapToProductDto(this Product model) =>
        new ProductDto(model.Id, model.Name, model.Description, model.Price, model.Stock);
}
