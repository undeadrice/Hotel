using Hotel.Application.Products.Services;
using Hotel.Application.Products.TransferObjects;
using MediatR;

namespace Hotel.Application.Products.Queries;

internal class GetProductsQueryHandler(IProductReadRepository productReadRepository)
    : IRequestHandler<GetProductsQuery, IReadOnlyCollection<ProductDto>>
{
    public async Task<IReadOnlyCollection<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        return await productReadRepository.GetAll(cancellationToken);
    }
}

