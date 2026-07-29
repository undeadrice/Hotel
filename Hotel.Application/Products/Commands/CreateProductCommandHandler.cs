using Hotel.Domain.Products.Entities;
using Hotel.Domain.Products.Services;
using MediatR;

namespace Hotel.Application.Products.Commands
{
    public class CreateProductCommandHandler(IProductRepository productRepository) : IRequestHandler<CreateProductCommand, Guid>
    {
        public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = Product.Create(request.Name, request.Description, request.Price, request.Stock);

            await productRepository.Add(product);

            return product.Id;
        }
    }
}