using Hotel.Application.Pipeline;

namespace Hotel.Application.Transactions.Commands;

public record CreateTransactionCodeCommand(
    Guid TransactionGroupId,
    string Code,
    string Name,
    string? Description,
    decimal DefaultAmount)
    : ICommand<Guid>;
