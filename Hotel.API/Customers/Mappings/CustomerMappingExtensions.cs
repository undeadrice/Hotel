using Hotel.API.Customers.Responses;
using Hotel.Application.Customers.TransferObjects;

namespace Hotel.API.Customers.Mappings;

public static class CustomerMappingExtensions
{
    public static CustomerResponse MapToCustomerResponse(this CustomerDto dto) =>
        new CustomerResponse(
            dto.Id,
            dto.FirstName,
            dto.LastName,
            dto.Phone,
            dto.Email,
            dto.DocumentNumber);

    public static CustomerListResponse MapToCustomerListResponse(this CustomerListDto dto) =>
        new CustomerListResponse(
            dto.Id,
            dto.FullName,
            dto.Phone,
            dto.Email,
            dto.DocumentNumber);
}