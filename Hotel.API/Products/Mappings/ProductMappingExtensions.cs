using Hotel.API.Products.Responses;
using Hotel.Application.Products.TransferObjects;

namespace Hotel.API.Products.Mappings;

public static class ProductMappingExtensions
{
    public static ProductResponse MapToProductResponse(this ProductDto dto) =>
        new ProductResponse(dto.Id, dto.Name, dto.Description, dto.Price, dto.Stock);
}