namespace Hotel.Application.Transactions.TransferObjects;

public record TransactionGroupDto(
    Guid Id,
    string Code,
    string Name,
    string? Description,
    int Type,
    bool IsActive,
    IReadOnlyCollection<TransactionCodeListDto> TransactionCodes);
