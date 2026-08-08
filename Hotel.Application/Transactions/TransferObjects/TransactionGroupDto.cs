namespace Hotel.Application.Transactions.TransferObjects;

public record TransactionGroupDto(
    Guid Id,
    string Code,
    string Name,
    int Type,
    bool IsActive,
    IReadOnlyCollection<TransactionCodeListDto> TransactionCodes);
