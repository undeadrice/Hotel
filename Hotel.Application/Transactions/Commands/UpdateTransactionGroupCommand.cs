using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;
using Hotel.Domain.Transactions.Enums;

namespace Hotel.Application.Transactions.Commands;

[CheckPermission(Permission.TransactionGroupEdit)]
public record UpdateTransactionGroupCommand(
    Guid Id,
    string Code,
    string Name,
    TransactionType Type)
    : ICommand;
