using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;

namespace Hotel.Application.Transactions.Commands;

[CheckPermission(Permission.TransactionCodeCreate)]
public record CreateTransactionCodeCommand(
    Guid TransactionGroupId,
    string Code,
    string Name)
    : ICommand<Guid>;
