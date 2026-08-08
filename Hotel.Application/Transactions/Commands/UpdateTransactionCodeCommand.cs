using Hotel.Application.Pipeline;

namespace Hotel.Application.Transactions.Commands;

public record UpdateTransactionCodeCommand(
    Guid Id,
    Guid TransactionGroupId,
    string Code,
    string Name)
    : ICommand;
