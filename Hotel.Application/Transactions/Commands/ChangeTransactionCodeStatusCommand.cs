using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;

namespace Hotel.Application.Transactions.Commands;

[CheckPermission(Permission.TransactionCodeEdit)]
public record ChangeTransactionCodeStatusCommand(
    Guid Id,
    bool IsActive)
    : ICommand;
