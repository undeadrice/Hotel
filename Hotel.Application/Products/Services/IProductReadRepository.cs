using Hotel.Application.Products.TransferObjects;

namespace Hotel.Application.Products.Services;

public interface IProductReadRepository
{
    Task<IReadOnlyCollection<ProductDto>> GetAll(CancellationToken cancellationToken);
}