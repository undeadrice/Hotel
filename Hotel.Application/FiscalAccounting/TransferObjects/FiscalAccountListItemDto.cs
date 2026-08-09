namespace Hotel.Application.FiscalAccounting.TransferObjects;

public record FiscalAccountListItemDto(
    Guid Id,
    DateTime CreatedAt,
    string OwnerFullName);