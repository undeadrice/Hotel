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
    IReadOnlyCollection<FolioItemDto> Items);

public record FolioItemDto(
    Guid Id,
    string Description,
    int Quantity,
    decimal Amount,
    Guid TransactionCodeId,
    DateTime CreatedAt);
