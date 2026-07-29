using Hotel.Application.Products.Services;
using Hotel.Application.Products.TransferObjects;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Persistence.Products;

public class ProductReadRepository(PersistenceDbContext dbContext) : IProductReadRepository
{
    public async Task<IReadOnlyCollection<ProductDto>> GetAll(CancellationToken cancellationToken)
    {
        return await dbContext.Products
            .AsNoTracking()
            .OrderBy(p => p.Name)
            .Select(p => new ProductDto(p.Id, p.Name, p.Description, p.Price, p.Stock))
            .ToListAsync(cancellationToken);
    }
}