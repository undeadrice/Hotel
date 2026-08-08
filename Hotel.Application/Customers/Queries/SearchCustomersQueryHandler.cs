using Hotel.Application.Customers.Services;
using Hotel.Application.Customers.TransferObjects;
using MediatR;

namespace Hotel.Application.Customers.Queries;

internal class SearchCustomersQueryHandler(ICustomerReadRepository customerReadRepository)
    : IRequestHandler<SearchCustomersQuery, IReadOnlyCollection<CustomerListDto>>
{
    public async Task<IReadOnlyCollection<CustomerListDto>> Handle(SearchCustomersQuery request, CancellationToken cancellationToken)
    {
        return await customerReadRepository.Search(
            request.Name,
            request.Phone,
            request.Email,
            request.DocumentNumber,
            cancellationToken);
    }
}