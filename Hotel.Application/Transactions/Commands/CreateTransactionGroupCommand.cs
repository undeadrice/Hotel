using Hotel.Application.Pipeline;
using Hotel.Domain.Transactions.Enums;

namespace Hotel.Application.Transactions.Commands;

public record CreateTransactionGroupCommand(
    string Code,
    string Name,
    string? Description,
    TransactionType Type)
    : ICommand<Guid>;
