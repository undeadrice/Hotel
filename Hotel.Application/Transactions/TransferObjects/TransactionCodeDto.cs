namespace Hotel.Application.Transactions.TransferObjects;

public record TransactionCodeDto(
    Guid Id,
    Guid TransactionGroupId,
    string TransactionGroupName,
    string Code,
    string Name,
    string? Description,
    decimal DefaultAmount,
    bool IsActive);
