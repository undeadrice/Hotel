using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;
using Hotel.Domain.Transactions.Enums;

namespace Hotel.Application.Transactions.Commands;

[CheckPermission(Permission.TransactionGroupCreate)]
public record CreateTransactionGroupCommand(
    string Code,
    string Name,
    TransactionType Type)
    : ICommand<Guid>;
