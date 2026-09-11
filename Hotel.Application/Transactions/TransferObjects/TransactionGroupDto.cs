using Hotel.Domain.Transactions.Enums;

namespace Hotel.Application.Transactions.TransferObjects;

public record TransactionGroupDto(
    Guid Id,
    string Code,
    string Name,
    TransactionType Type,
    bool IsActive,
    IReadOnlyCollection<TransactionCodeListDto> TransactionCodes);
