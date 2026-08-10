using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;

namespace Hotel.Application.Transactions.Commands;

[CheckPermission(Permission.TransactionGroupEdit)]
public record ChangeTransactionGroupStatusCommand(
    Guid Id,
    bool IsActive)
    : ICommand;
