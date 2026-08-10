using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;

namespace Hotel.Application.Transactions.Commands;

[CheckPermission(Permission.TransactionCodeEdit)]
public record UpdateTransactionCodeCommand(
    Guid Id,
    Guid TransactionGroupId,
    string Code,
    string Name)
    : ICommand;
