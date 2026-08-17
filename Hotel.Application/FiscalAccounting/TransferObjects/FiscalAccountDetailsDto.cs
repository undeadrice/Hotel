using Hotel.Domain.FiscalAccounting.Enums;
using Hotel.Domain.Transactions.Enums;

namespace Hotel.Application.FiscalAccounting.TransferObjects;

public record FiscalAccountDetailsDto(
    Guid Id,
    Guid OriginatorId,
    string CycleIdentifier,
    string OwnerFullName,
    DateTime CreatedAt,
    IReadOnlyCollection<FolioDto> Folios);

public record FolioDto(
    Guid Id,
    DateTime CreatedAt,
    FolioStatus Status,
    IReadOnlyCollection<FolioItemDto> Items);

public record FolioItemDto(
    Guid Id,
    string Description,
    int Quantity,
    decimal Amount,
    decimal TotalAmount,
    Guid TransactionCodeId,
    TransactionType TransactionGroupType,
    DateOnly BusinessDate,
    DateTime CreatedAt);
