using Hotel.Application.Products.TransferObjects;
using MediatR;

namespace Hotel.Application.Products.Queries;

public record GetProductsQuery() : IRequest<IReadOnlyCollection<ProductDto>>;

