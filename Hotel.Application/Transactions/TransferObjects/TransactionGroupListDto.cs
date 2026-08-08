using Hotel.Domain.Transactions.Enums;

namespace Hotel.Application.Transactions.TransferObjects;

public record TransactionGroupListDto(
    Guid Id,
    string Code,
    string Name,
    TransactionType Type,
    bool IsActive,
    int TransactionCodesCount);
