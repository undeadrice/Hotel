using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;

namespace Hotel.Application.FiscalAccounting.Commands;

[CheckPermission(Permission.FiscalAccountEdit)]
public record CreateFolioItemCommand(
    Guid FolioId,
    string Description,
    int Quantity,
    decimal Amount,
    Guid TransactionCodeId)
    : ICommand<Guid>;