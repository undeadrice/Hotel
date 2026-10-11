using Hotel.Domain.FiscalAccounting.Enums;

namespace Hotel.Application.FiscalAccounting.TransferObjects;

public record FiscalAccountListItemDto(
    Guid Id,
    string CycleIdentifier,
    DateTime CreatedAt,
    string OwnerFullName,
    FiscalAccountStatus Status);
