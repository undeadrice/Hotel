namespace Hotel.Application.Transactions.TransferObjects;

public record TransactionGroupListDto(
    Guid Id,
    string Code,
    string Name,
    int Type,
    bool IsActive,
    int TransactionCodesCount);
