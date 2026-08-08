using Hotel.Application.Customers.TransferObjects;

namespace Hotel.Application.Customers.Services;

public interface ICustomerReadRepository
{
    Task<IReadOnlyCollection<CustomerListDto>> GetAll(CancellationToken cancellationToken);

    Task<CustomerDto> GetById(Guid id, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<CustomerListDto>> Search(
        string? name,
        string? phone,
        string? email,
        string? documentNumber,
        CancellationToken cancellationToken);
}