using Hotel.Application.Pipeline;

namespace Hotel.Application.Transactions.Commands;

public record ChangeTransactionGroupStatusCommand(
    Guid Id,
    bool IsActive)
    : ICommand;
