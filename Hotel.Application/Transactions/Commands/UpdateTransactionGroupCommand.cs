using Hotel.Application.Pipeline;
using Hotel.Domain.Transactions.Enums;

namespace Hotel.Application.Transactions.Commands;

public record UpdateTransactionGroupCommand(
    Guid Id,
    string Code,
    string Name,
    TransactionType Type)
    : ICommand;
