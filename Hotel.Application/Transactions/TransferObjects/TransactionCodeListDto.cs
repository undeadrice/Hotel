namespace Hotel.Application.Transactions.TransferObjects;

public record TransactionCodeListDto(
    Guid Id,
    Guid TransactionGroupId,
    string TransactionGroupName,
    string Code,
    string Name,
    bool IsActive);
